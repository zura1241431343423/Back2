using E_commerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<SearchController> _logger;

        public SearchController(DataContext context, ILogger<SearchController> logger)
        {
            _context = context;
            _logger = logger;
        }

 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSearchResult>>> SearchProducts(
            [FromQuery] string query,
            [FromQuery] int limit = 6)
        {
            _logger.LogInformation($"Search request received - Query: '{query}', Limit: {limit}");

            if (string.IsNullOrWhiteSpace(query))
            {
                _logger.LogWarning("Empty search query received");
                return BadRequest("Search query cannot be empty");
            }

            try
            {
                
                var totalProducts = await _context.Products.CountAsync();
                _logger.LogInformation($"Total products in database: {totalProducts}");

                if (totalProducts == 0)
                {
                    _logger.LogWarning("No products found in database");
                    return Ok(new List<ProductSearchResult>());
                }

            
                var products = await _context.Products
                    .Where(p => p.Name.ToLower().Contains(query.ToLower()))
                    .Take(limit)
                    .Select(p => new ProductSearchResult
                    {
                        Id = p.Id,
                        Name = p.Name ?? "",
                        Price = p.Price,
                        Images = p.Images ?? new string[0], 
                        Category = p.Category ?? "",
                        
                        AverageRating = p.RatingCount > 0 ? Math.Round((double)p.RatingSum / p.RatingCount, 2) : 0.0,
                        RatingCount = p.RatingCount
                    })
                    .ToListAsync();

                _logger.LogInformation($"Found {products.Count} products matching query '{query}'");

                if (products.Any())
                {
                    var productNames = string.Join(", ", products.Take(3).Select(p => p.Name));
                    _logger.LogInformation($"Sample results: {productNames}");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while searching for products with query: '{query}'");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("suggestions")]
        public async Task<ActionResult<IEnumerable<string>>> GetSearchSuggestions(
            [FromQuery] string query,
            [FromQuery] int limit = 5)
        {
            _logger.LogInformation($"Suggestions request received - Query: '{query}', Limit: {limit}");

            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty");
            }

            try
            {
                var suggestions = await _context.Products
                    .Where(p => p.Name.ToLower().Contains(query.ToLower()))
                    .Select(p => p.Name)
                    .Distinct()
                    .Take(limit)
                    .ToListAsync();

                _logger.LogInformation($"Found {suggestions.Count} suggestions for query '{query}'");
                return Ok(suggestions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting suggestions for query: '{query}'");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Search API is working", timestamp = DateTime.UtcNow });
        }

       
        public class ProductSearchResult
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public decimal Price { get; set; }
            public string[] Images { get; set; } = new string[0];
            public string Category { get; set; } = "";
            public double AverageRating { get; set; }
            public int RatingCount { get; set; }
        }
    }
}