using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly DataContext _context;

        public RatingController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Rating>>> GetProductRatings(int productId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .OrderByDescending(r => r.RatedAt)
                .ToListAsync();

            return Ok(ratings);
        }

        [HttpGet("product/{productId}/average")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetProductAverageRating(int productId)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            double averageRating = product.RatingCount > 0
                ? Math.Round((double)product.RatingSum / product.RatingCount, 2)
                : 0;

            return Ok(new
            {
                ProductId = productId,
                AverageRating = averageRating,
                RatingCount = product.RatingCount,
                RatingSum = product.RatingSum
            });
        }
        [HttpGet("top-rated")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Product>>> GetTopRatedProducts()
        {
            var topRatedProducts = await _context.Products
                .Where(p => p.AverageRating > 0) 
                .OrderByDescending(p => p.AverageRating)
                .ThenByDescending(p => p.RatingCount) 
                .Take(20) 
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Brand,
                    p.Price,
                    p.DiscountedPrice,
                    p.Category,
                    p.SubCategory,
                    p.Images,
                    AverageRating = Math.Round(p.AverageRating, 2),
                    RatingCount = p.RatingCount,
                    p.RatingSum
                })
                .ToListAsync();

            return Ok(topRatedProducts);
        }

        [HttpGet("user/{userId}/product/{productId}")]
        public async Task<ActionResult<Rating>> GetUserRatingForProduct(int userId, int productId)
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == productId);

            if (rating == null)
            {
                return NotFound("Rating not found");
            }

            return Ok(rating);
        }

        [HttpPost]
        public async Task<ActionResult<Rating>> CreateRating([FromBody] RatingDto ratingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = GetCurrentUserId();

            var product = await _context.Products.Include(p => p.Ratings)
                                                 .FirstOrDefaultAsync(p => p.Id == ratingDto.ProductId);
            if (product == null)
                return NotFound("Product not found");

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == currentUserId && r.ProductId == ratingDto.ProductId);

            if (existingRating != null)
                return BadRequest("You have already rated this product. Use PUT to update your rating.");

            if (ratingDto.Rating < 1 || ratingDto.Rating > 5)
                return BadRequest("Rating must be between 1 and 5");

            var rating = new Rating
            {
                ProductId = ratingDto.ProductId,
                UserId = currentUserId,
                Value = ratingDto.Rating,
                RatedAt = DateTime.UtcNow
            };

            _context.Ratings.Add(rating);

            product.RatingSum += ratingDto.Rating;
            product.RatingCount += 1;
            product.AverageRating = Math.Round((double)product.RatingSum / product.RatingCount, 2);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserRatingForProduct),
                new { userId = currentUserId, productId = ratingDto.ProductId }, rating);
        }

        [HttpPut("product/{productId}")]
        public async Task<IActionResult> UpdateRating(int productId, [FromBody] RatingDto ratingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (productId != ratingDto.ProductId)
                return BadRequest("Product ID mismatch");

            var currentUserId = GetCurrentUserId();

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == currentUserId && r.ProductId == productId);

            if (existingRating == null)
                return NotFound("Rating not found. Use POST to create a new rating.");

            if (ratingDto.Rating < 1 || ratingDto.Rating > 5)
                return BadRequest("Rating must be between 1 and 5");

            var product = await _context.Products.Include(p => p.Ratings)
                                                 .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                return NotFound("Product not found");

            product.RatingSum = product.RatingSum - existingRating.Value + ratingDto.Rating;
            existingRating.Value = ratingDto.Rating;
            existingRating.RatedAt = DateTime.UtcNow;

            product.AverageRating = product.RatingCount > 0
                ? Math.Round((double)product.RatingSum / product.RatingCount, 2)
                : 0;

            await _context.SaveChangesAsync();

            return Ok(existingRating);
        }

        [HttpDelete("product/{productId}")]
        public async Task<IActionResult> DeleteRating(int productId)
        {
            var currentUserId = GetCurrentUserId();

            var rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == currentUserId && r.ProductId == productId);

            if (rating == null)
                return NotFound("Rating not found");

            var product = await _context.Products.Include(p => p.Ratings)
                                                 .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                return NotFound("Product not found");

            product.RatingSum -= rating.Value;
            product.RatingCount = Math.Max(product.RatingCount - 1, 0);

            _context.Ratings.Remove(rating);

            product.AverageRating = product.RatingCount > 0
                ? Math.Round((double)product.RatingSum / product.RatingCount, 2)
                : 0;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("user/my-ratings")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetMyRatings()
        {
            var currentUserId = GetCurrentUserId();

            var ratings = await _context.Ratings
                .Where(r => r.UserId == currentUserId)
                .Include(r => r.Product)
                .OrderByDescending(r => r.RatedAt)
                .ToListAsync();

            return Ok(ratings);
        }

        [HttpGet("product/{productId}/stats")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetProductRatingStats(int productId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            if (!ratings.Any())
            {
                return Ok(new
                {
                    ProductId = productId,
                    TotalRatings = 0,
                    AverageRating = 0.0,
                    RatingDistribution = new int[5]
                });
            }

            var ratingDistribution = new int[5];
            foreach (var rating in ratings)
            {
                ratingDistribution[rating.Value - 1]++;
            }

            return Ok(new
            {
                ProductId = productId,
                TotalRatings = ratings.Count,
                AverageRating = Math.Round(ratings.Average(r => r.Value), 2),
                RatingDistribution = ratingDistribution
            });
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            return userId;
        }
    }
}
