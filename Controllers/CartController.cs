using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_commerce.Models;
using E_commerce.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly DataContext _context;

        public CartController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetAll()
        {
            var cartItems = await _context.CartItems.ToListAsync();
            return Ok(cartItems);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetByUserId(int userId)
        {
            var userCartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();
            return Ok(userCartItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartItem>> GetById(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<CartItem>> AddToCart([FromBody] CartItem cartItem)
        {
            // Validation
            if (cartItem.UserId <= 0 || cartItem.ProductId <= 0)
                return BadRequest("Invalid UserId or ProductId");

            if (cartItem.Quantity <= 0)
                return BadRequest("Quantity must be greater than 0");

            // Check if item already exists for this user and product
            var existing = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == cartItem.ProductId && c.UserId == cartItem.UserId);

            if (existing != null)
            {
                // Update existing item
                if (existing.Quantity + cartItem.Quantity > cartItem.QuantityAvailable)
                    return BadRequest($"Cannot exceed available quantity ({cartItem.QuantityAvailable})");

                existing.Quantity += cartItem.Quantity;
                existing.AddedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(existing);
            }
            else
            {
                // Add new item
                if (cartItem.Quantity > cartItem.QuantityAvailable)
                    return BadRequest($"Cannot exceed available quantity ({cartItem.QuantityAvailable})");

                cartItem.AddedAt = DateTime.UtcNow;
                _context.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
                return Ok(cartItem);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuantity(int id, [FromBody] int newQuantity)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null)
                return NotFound();

            if (newQuantity > item.QuantityAvailable)
                return BadRequest($"Cannot exceed available quantity ({item.QuantityAvailable})");

            if (newQuantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = newQuantity;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> ClearCart()
        {
            var allItems = await _context.CartItems.ToListAsync();
            _context.CartItems.RemoveRange(allItems);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("total/{userId}")]
        public async Task<ActionResult<decimal>> GetCartTotal(int userId)
        {
            var total = await _context.CartItems
                .Where(item => item.UserId == userId)
                .SumAsync(item => item.Price * item.Quantity);
            return Ok(total);
        }

        [HttpGet("count/{userId}")]
        public async Task<ActionResult<int>> GetCartCount(int userId)
        {
            var count = await _context.CartItems
                .Where(item => item.UserId == userId)
                .SumAsync(item => item.Quantity);
            return Ok(count);
        }

        [HttpDelete("user/{userId}")]
        public async Task<ActionResult> ClearUserCart(int userId)
        {
            var userItems = await _context.CartItems
                .Where(item => item.UserId == userId)
                .ToListAsync();

            _context.CartItems.RemoveRange(userItems);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("update-item/{id}")]
        public async Task<ActionResult<CartItem>> UpdateCartItem(int id, [FromBody] CartItem updatedItem)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null)
                return NotFound();

            if (updatedItem.Quantity > updatedItem.QuantityAvailable)
                return BadRequest($"Cannot exceed available quantity ({updatedItem.QuantityAvailable})");

            // Update properties
            item.Name = updatedItem.Name;
            item.Price = updatedItem.Price;
            item.Quantity = updatedItem.Quantity;
            item.QuantityAvailable = updatedItem.QuantityAvailable;

            await _context.SaveChangesAsync();
            return Ok(item);
        }

        // Helper method to get cart item by userId and productId
        [HttpGet("user/{userId}/product/{productId}")]
        public async Task<ActionResult<CartItem>> GetByUserIdAndProductId(int userId, int productId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
    }
}