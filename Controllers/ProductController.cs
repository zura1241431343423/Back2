using System.Globalization;
using System.Security.Claims;
using E_commerce.Data;
using E_commerce.DTOs;
using E_commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(DataContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Ratings)
                    .Include(p => p.CategoryEntity)
                    .Include(p => p.SubcategoryEntity)
                    .ToListAsync();

                var sorted = products.OrderByDescending(p =>
                    p.RatingCount == 0 ? 0 : (double)p.RatingSum / p.RatingCount).ToList();

                return Ok(sorted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product list");
                return StatusCode(500, "Internal server error while fetching products");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Ratings)
                    .Include(p => p.CategoryEntity)
                    .Include(p => p.SubcategoryEntity)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product with ID {ProductId}", id);
                return StatusCode(500, "Internal server error while fetching product");
            }
        }

        [HttpGet("newly-added")]
        public async Task<ActionResult<IEnumerable<Product>>> GetNewlyAddedProducts(
            [FromQuery] int maxProducts = 20,
            [FromQuery] int daysBack = 30)
        {
            try
            {
                if (maxProducts <= 0)
                    return BadRequest("maxProducts must be greater than 0");

                if (daysBack <= 0)
                    return BadRequest("daysBack must be greater than 0");

                var cutoffDate = DateTime.UtcNow.AddDays(-daysBack);

                var newlyAddedProducts = await _context.Products
                    .Include(p => p.Ratings)
                    .Include(p => p.CategoryEntity)
                    .Include(p => p.SubcategoryEntity)
                    .Where(p => p.CreatedAt >= cutoffDate)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(maxProducts)
                    .ToListAsync();

                return Ok(newlyAddedProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching newly added products with maxProducts: {MaxProducts}, daysBack: {DaysBack}", maxProducts, daysBack);
                return StatusCode(500, "Internal server error while fetching newly added products");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductCreateUpdateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid product data");

            var product = new Product
            {
                Name = dto.Name,
                Brand = dto.Brand,
                Price = dto.Price,
                Quantity = dto.Quantity,
                Warranty = dto.Warranty,
                Images = dto.Images,
                CreatedAt = DateTime.UtcNow
            };

            // Handle category
            if (!string.IsNullOrWhiteSpace(dto.Category))
            {
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == dto.Category.ToLower().Trim());

                if (existingCategory == null)
                {
                    existingCategory = new Category
                    {
                        Name = dto.Category.Trim(),
                        Description = $"Auto-created category for {dto.Category}",
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Categories.Add(existingCategory);
                    await _context.SaveChangesAsync();
                }

                product.CategoryId = existingCategory.Id;
                product.Category = existingCategory.Name;
            }

            // Handle subcategory
            if (!string.IsNullOrWhiteSpace(dto.SubCategory) && product.CategoryId.HasValue)
            {
                var existingSubcategory = await _context.Subcategories
                    .FirstOrDefaultAsync(sc => sc.Name.ToLower().Trim() == dto.SubCategory.ToLower().Trim()
                                            && sc.CategoryId == product.CategoryId.Value);

                if (existingSubcategory == null)
                {
                    existingSubcategory = new Subcategory
                    {
                        Name = dto.SubCategory.Trim(),
                        Description = $"Auto-created subcategory for {dto.SubCategory}",
                        CategoryId = product.CategoryId.Value,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Subcategories.Add(existingSubcategory);
                    await _context.SaveChangesAsync();
                }

                product.SubcategoryId = existingSubcategory.Id;
                product.SubCategory = existingSubcategory.Name;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null || id != updatedProduct.Id)
                return BadRequest("Product ID mismatch or null input");

            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                    return NotFound();

                existingProduct.Name = updatedProduct.Name;
                existingProduct.Brand = updatedProduct.Brand;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.Category = updatedProduct.Category;
                existingProduct.SubCategory = updatedProduct.SubCategory;
                existingProduct.CategoryId = updatedProduct.CategoryId;
                existingProduct.SubcategoryId = updatedProduct.SubcategoryId;
                existingProduct.Quantity = updatedProduct.Quantity;
                existingProduct.Warranty = updatedProduct.Warranty;
                existingProduct.Images = updatedProduct.Images;
                existingProduct.DiscountedPrice = updatedProduct.DiscountedPrice;
                existingProduct.DiscountPercentage = updatedProduct.DiscountPercentage;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                    return NotFound();

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", id);
                return StatusCode(500, "Internal server error while updating product");
            }
        }

        [HttpPut("{id}/discount")]
        public async Task<IActionResult> ApplyDiscount(int id, [FromBody] int discountPercentage)
        {
            try
            {
                if (discountPercentage < 1 || discountPercentage > 100)
                    return BadRequest("Discount percentage must be between 1 and 100");

                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                product.ApplyDiscount(discountPercentage);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    discountPercentage = product.DiscountPercentage,
                    discountedPrice = product.DiscountedPrice
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying discount to product {ProductId}", id);
                return StatusCode(500, "Internal server error while applying discount");
            }
        }

        [HttpDelete("{id}/discount")]
        public async Task<IActionResult> RemoveDiscount(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                product.RemoveDiscount();
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Discount removed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing discount from product {ProductId}", id);
                return StatusCode(500, "Internal server error while removing discount");
            }
        }

        [HttpPut("bulk-update")]
        public async Task<IActionResult> BulkUpdateProducts([FromBody] List<Product> products)
        {
            try
            {
                if (products == null || !products.Any())
                    return BadRequest("Product list is empty");

                foreach (var updatedProduct in products)
                {
                    var existingProduct = await _context.Products.FindAsync(updatedProduct.Id);
                    if (existingProduct != null)
                    {
                        existingProduct.Quantity = updatedProduct.Quantity;
                        existingProduct.DiscountedPrice = updatedProduct.DiscountedPrice;
                        existingProduct.DiscountPercentage = updatedProduct.DiscountPercentage;
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Products updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk updating products");
                return StatusCode(500, "Internal server error while updating products");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", id);
                return StatusCode(500, "Internal server error while deleting product");
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            try
            {
                var categories = await _context.Products
                    .Where(p => !string.IsNullOrEmpty(p.Category))
                    .Select(p => p.Category.Trim())
                    .Distinct()
                    .OrderBy(c => c)
                    .ToListAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                return StatusCode(500, "Internal server error while fetching categories");
            }
        }

        [HttpGet("categories-with-subcategories")]
        public async Task<ActionResult<List<CategoryWithSubcategoriesDto>>> GetCategoriesWithSubcategories()
        {
            try
            {
                var products = await _context.Products
                    .Where(p => !string.IsNullOrWhiteSpace(p.Category))
                    .ToListAsync();

                var grouped = products
                    .GroupBy(p => p.Category.Trim().ToLower())
                    .Select(g =>
                    {
                        var originalCategory = g.First().Category.Trim();
                        var subcategories = g
                            .Select(p => string.IsNullOrWhiteSpace(p.SubCategory) ? null : p.SubCategory.Trim())
                            .Where(sc => !string.IsNullOrWhiteSpace(sc))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .Select(sc => new SubcategoryDto
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = sc!
                            })
                            .ToList();

                        return new CategoryWithSubcategoriesDto
                        {
                            Category = originalCategory,
                            Subcategories = subcategories
                        };
                    })
                    .ToList();

                return Ok(grouped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories with subcategories");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("by-category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category))
                    return BadRequest("Category parameter is required");

                var products = await _context.Products
                    .Include(p => p.Ratings)
                    .Include(p => p.CategoryEntity)
                    .Include(p => p.SubcategoryEntity)
                    .Where(p => p.Category.ToLower().Trim() == category.ToLower().Trim())
                    .ToListAsync();

                if (!products.Any())
                    return Ok(new List<Product>());

                var sorted = products.OrderByDescending(p =>
                    p.RatingCount == 0 ? 0 : (double)p.RatingSum / p.RatingCount).ToList();

                return Ok(sorted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products for category {Category}", category);
                return StatusCode(500, "Internal server error while fetching products by category");
            }
        }

        [HttpGet("by-subcategory/{subcategory}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsBySubcategory(string subcategory)
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Ratings)
                    .Include(p => p.CategoryEntity)
                    .Include(p => p.SubcategoryEntity)
                    .Where(p => p.SubCategory.ToLower().Trim() == subcategory.ToLower().Trim())
                    .ToListAsync();

                if (!products.Any())
                    return Ok(new List<Product>());

                var sorted = products.OrderByDescending(p =>
                    p.RatingCount == 0 ? 0 : (double)p.RatingSum / p.RatingCount).ToList();

                return Ok(sorted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products for subcategory {Subcategory}", subcategory);
                return StatusCode(500, "Internal server error while fetching products by subcategory");
            }
        }

        [HttpGet("brands/by-category/{category}")]
        public async Task<ActionResult<IEnumerable<string>>> GetBrandsByCategory(string category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category))
                    return BadRequest("Category parameter is required");

                var brands = await _context.Products
                    .Where(p => p.Category.ToLower().Trim() == category.ToLower().Trim() &&
                               !string.IsNullOrWhiteSpace(p.Brand))
                    .Select(p => p.Brand.Trim())
                    .Distinct()
                    .OrderBy(b => b)
                    .ToListAsync();

                return Ok(brands);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching brands for category {Category}", category);
                return StatusCode(500, "Internal server error while fetching brands by category");
            }
        }

        [HttpPut("{id}/partial-update")]
        public async Task<IActionResult> UpdateProductPartial(int id, [FromBody] ProductPartialUpdateDto updateDto)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound($"Product with ID {id} not found.");

                if (updateDto.Quantity.HasValue)
                    product.Quantity = updateDto.Quantity.Value;

                if (updateDto.DiscountPercentage.HasValue)
                {
                    if (updateDto.DiscountPercentage.Value == 0)
                        product.RemoveDiscount();
                    else
                        product.ApplyDiscount(updateDto.DiscountPercentage.Value);
                }
                else if (updateDto.RemoveDiscount == true)
                {
                    product.RemoveDiscount();
                }

                await _context.SaveChangesAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating product: {ex.Message}");
            }
        }

        
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
public class SubcategoryDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class CategoryWithSubcategoriesDto
{
    public string Category { get; set; }
    public List<SubcategoryDto> Subcategories { get; set; }
}

public class ProductPartialUpdateDto
{
    public int? Quantity { get; set; }
    public int? DiscountPercentage { get; set; }
    public bool? RemoveDiscount { get; set; }
}
    

