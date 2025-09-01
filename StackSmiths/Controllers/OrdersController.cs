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
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
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
                return BadRequest("Cart is empty");

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

            return Ok(new
            {
                Message = "Order placed successfully",
                OrderId = order.Id,
                Total = order.TotalAmount,
                Items = order.Items.Select(i => new
                {
                    i.ProductId,
                    i.Quantity,
                    i.Price
                })
            });
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetOrderHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _context.Orders!
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return Ok(orders);
        }
    }
}
