using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_commerce.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class ClickController : ControllerBase
    {
        private readonly DataContext _context;

        public ClickController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> TrackClick([FromBody] ClickDto clickDto)
        {
            var product = await _context.Products.FindAsync(clickDto.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var click = new Click
            {
                ProductId = clickDto.ProductId,
                UserId = clickDto.UserId,
                SubCategory = clickDto.SubCategory ?? product.SubCategory,
                ClickedAt = DateTime.UtcNow
            };

            _context.Clicks.Add(click);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Click tracked successfully." });
        }

        [HttpGet("recommendations/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRecommendations(int userId, [FromQuery] int limit = 50)
        {
            try
            {
               
                var hasClicks = await _context.Clicks.AnyAsync(c => c.UserId == userId);

                if (!hasClicks)
                {
                    
                    return Ok(new List<object>());
                }

                var clickStats = await _context.Clicks
                    .Where(c => c.UserId == userId)
                    .GroupBy(c => c.SubCategory)
                    .Select(g => new
                    {
                        SubCategory = g.Key,
                        ClickCount = g.Count(),
                        LastClickedAt = g.Max(c => c.ClickedAt)
                    })
                    .OrderByDescending(s => s.ClickCount)
                    .ThenByDescending(s => s.LastClickedAt)
                    .ToListAsync();

                if (!clickStats.Any())
                {
                    return Ok(new List<object>());
                }

                var recommendations = new List<object>();

                foreach (var stat in clickStats)
                {
                    
                    var recentlyClickedProductIds = await _context.Clicks
                        .Where(c => c.UserId == userId && c.SubCategory == stat.SubCategory)
                        .OrderByDescending(c => c.ClickedAt)
                        .Take(10) 
                        .Select(c => c.ProductId)
                        .ToListAsync();

                    var productsInCategory = await _context.Products
                        .Where(p => p.SubCategory == stat.SubCategory &&
                                   !recentlyClickedProductIds.Contains(p.Id))
                        .OrderBy(p => Guid.NewGuid()) 
                        .Take(Math.Max(1, limit / clickStats.Count + 5)) 
                        .ToListAsync();

                    foreach (var product in productsInCategory)
                    {
                        recommendations.Add(new
                        {
                            Product = product,
                            SubCategory = stat.SubCategory,
                            ClickCount = stat.ClickCount,
                            Confidence = CalculateConfidence(stat.ClickCount)
                        });

                        if (recommendations.Count >= limit)
                            break;
                    }

                    if (recommendations.Count >= limit)
                        break;
                }

              
                if (recommendations.Count < limit)
                {
                    var topCategories = clickStats.Take(3).Select(s => s.SubCategory).ToList();
                    var existingProductIds = recommendations.Select(r => {
                        var recObj = r.GetType().GetProperty("Product")?.GetValue(r);
                        return recObj?.GetType().GetProperty("Id")?.GetValue(recObj);
                    }).Where(id => id != null).Cast<int>().ToList();

                    var additionalProducts = await _context.Products
                        .Where(p => topCategories.Contains(p.SubCategory))
                        .Where(p => !existingProductIds.Contains(p.Id))
                        .OrderBy(p => Guid.NewGuid())
                        .Take(limit - recommendations.Count)
                        .ToListAsync();

                    foreach (var product in additionalProducts)
                    {
                        var categoryStats = clickStats.First(s => s.SubCategory == product.SubCategory);
                        recommendations.Add(new
                        {
                            Product = product,
                            SubCategory = product.SubCategory,
                            ClickCount = categoryStats.ClickCount,
                            Confidence = CalculateConfidence(categoryStats.ClickCount)
                        });
                    }
                }

             
                var shuffledRecommendations = recommendations
                    .OrderBy(r => Guid.NewGuid())
                    .Take(limit)
                    .ToList();

                return Ok(shuffledRecommendations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving recommendations", error = ex.Message });
            }
        }

        [HttpGet("user-stats/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserClickStats(int userId)
        {
            try
            {
                var clickStats = await _context.Clicks
                    .Where(c => c.UserId == userId)
                    .GroupBy(c => c.SubCategory)
                    .Select(g => new
                    {
                        SubCategory = g.Key,
                        ClickCount = g.Count(),
                        LastClickedAt = g.Max(c => c.ClickedAt)
                    })
                    .OrderByDescending(s => s.ClickCount)
                    .ToListAsync();

                return Ok(clickStats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving user stats", error = ex.Message });
            }
        }

      
        [HttpGet("has-clicks/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> HasUserClicks(int userId)
        {
            try
            {
                var hasClicks = await _context.Clicks.AnyAsync(c => c.UserId == userId);
                return Ok(new { hasClicks });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error checking user clicks", error = ex.Message });
            }
        }

        private double CalculateConfidence(int clickCount)
        {
            if (clickCount == 0) return 0;
            if (clickCount >= 10) return 1;
            return clickCount / 10.0;
        }

        [HttpGet("products/random")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRandomProducts([FromQuery] int limit = 20)
        {
            try
            {
                var randomProducts = await _context.Products
                    .OrderBy(p => Guid.NewGuid())
                    .Take(limit)
                    .ToListAsync();

                return Ok(randomProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving random products", error = ex.Message });
            }
        }

        [HttpGet("products/by-subcategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsBySubcategories([FromQuery] string subcategories, [FromQuery] int limit = 20)
        {
            try
            {
                var subcategoryList = subcategories.Split(',').ToList();
                var products = await _context.Products
                    .Where(p => subcategoryList.Contains(p.SubCategory))
                    .OrderBy(p => Guid.NewGuid())
                    .Take(limit)
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving products by subcategories", error = ex.Message });
            }
        }
    }
}