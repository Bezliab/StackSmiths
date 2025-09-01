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
    public class WishlistController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WishlistController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToWishlist([FromBody] WishlistItem item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            item.UserId = userId!;

            var exists = await _context.WishlistItems!
                .AnyAsync(w => w.UserId == userId && w.ProductId == item.ProductId);

            if (exists) return BadRequest("Item already in wishlist");

            _context.WishlistItems!.Add(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var wishlist = await _context.WishlistItems!
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();

            return Ok(wishlist);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveFromWishlist(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var item = await _context.WishlistItems!
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (item == null) return NotFound();

            _context.WishlistItems!.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }
    }
}
