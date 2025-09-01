using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackSmiths.Data;
using StackSmiths.Models;
using System.Security.Claims;

namespace StackSmiths.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItems item)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();


            var existing = await _context.CartItems!
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == item.ProductId);


            if (existing != null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                _context.CartItems!.Add(item);
            }

            await _context.SaveChangesAsync();
            return Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _context.CartItems!
                .Include(c => c.Products)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return Ok(cart);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = await _context.CartItems!
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (item == null) 
            { 
                return NotFound(); 
            }

            _context.CartItems!.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = await _context.CartItems!
                .Include(c => c.Products)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return BadRequest("Cart is empty.");

            var order = new Orders
            {
                UserId = userId!,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cartItems.Sum(c => c.Products.Price * c.Quantity),
                Items = cartItems.Select(c => new OrderItems
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Price = c.Products.Price
                }).ToList()
            };

            _context.Orders!.Add(order);

            _context.CartItems!.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Order placed successfully!", OrderId = order.Id });
        }
    }
}
