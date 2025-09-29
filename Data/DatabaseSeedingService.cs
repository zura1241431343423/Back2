using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Services
{
    public class DatabaseSeedingService
    {
        private readonly DataContext _context;
        private readonly ILogger<DatabaseSeedingService> _logger;

        public DatabaseSeedingService(DataContext context, ILogger<DatabaseSeedingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedProductsAsync()
        {
            try
            {
                if (await _context.Products.AnyAsync())
                {
                    _logger.LogInformation("Products already exist. Skipping seeding.");
                    return;
                }

                // 1️⃣ Get all products from abstractions
                var allProducts = It_EquipmentAbstractions.GetProducts()
                                  .Concat(AppliancesAbstractions.GetProducts())
                                  .Concat(Mobile_devicesAbstractions.GetProducts())
                                  .ToList();

                // 2️⃣ Ensure categories exist
                var categoryNames = allProducts.Select(p => p.Category)
                                              .Distinct()
                                              .Where(c => !string.IsNullOrWhiteSpace(c));

                foreach (var catName in categoryNames)
                {
                    var existingCategory = await _context.Categories
                        .FirstOrDefaultAsync(c => c.Name.ToLower() == catName.ToLower());
                    if (existingCategory == null)
                    {
                        var newCat = new Category
                        {
                            Name = catName,
                            Description = $"Auto-created for {catName}",
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.Categories.Add(newCat);
                        await _context.SaveChangesAsync();
                    }
                }

                // 3️⃣ Ensure subcategories exist and assign IDs to products
                foreach (var product in allProducts)
                {
                    if (!string.IsNullOrWhiteSpace(product.SubCategory))
                    {
                        var category = await _context.Categories.FirstAsync(c => c.Name == product.Category);

                        var existingSub = await _context.Subcategories
                            .FirstOrDefaultAsync(sc => sc.Name == product.SubCategory && sc.CategoryId == category.Id);

                        if (existingSub == null)
                        {
                            var newSub = new Subcategory
                            {
                                Name = product.SubCategory,
                                CategoryId = category.Id,
                                Description = $"Auto-created for {product.SubCategory}",
                                CreatedAt = DateTime.UtcNow
                            };
                            _context.Subcategories.Add(newSub);
                            await _context.SaveChangesAsync();
                        }

                        // Assign IDs to product
                        product.CategoryId = category.Id;
                        product.SubcategoryId = (await _context.Subcategories
                            .FirstAsync(sc => sc.Name == product.SubCategory && sc.CategoryId == category.Id)).Id;
                    }
                    else if (!string.IsNullOrWhiteSpace(product.Category))
                    {
                        // If product has only category
                        var category = await _context.Categories.FirstAsync(c => c.Name == product.Category);
                        product.CategoryId = category.Id;
                    }
                }

                // 4️⃣ Add all products
                _context.Products.AddRange(allProducts);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully seeded {allProducts.Count} products with categories/subcategories.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding products.");
                throw;
            }
        }

        public async Task SeedOrUpdateProductsAsync()
        {
            try
            {
                var allProducts = It_EquipmentAbstractions.GetProducts()
                                  .Concat(AppliancesAbstractions.GetProducts())
                                  .Concat(Mobile_devicesAbstractions.GetProducts())
                                  .ToList();

                foreach (var product in allProducts)
                {
                    // Assign category/subcategory IDs
                    if (!string.IsNullOrWhiteSpace(product.Category))
                    {
                        var category = await _context.Categories
                            .FirstOrDefaultAsync(c => c.Name.ToLower() == product.Category.ToLower());
                        if (category != null) product.CategoryId = category.Id;
                    }
                    if (!string.IsNullOrWhiteSpace(product.SubCategory) && product.CategoryId.HasValue)
                    {
                        var subcategory = await _context.Subcategories
                            .FirstOrDefaultAsync(sc => sc.Name.ToLower() == product.SubCategory.ToLower()
                                                       && sc.CategoryId == product.CategoryId.Value);
                        if (subcategory != null) product.SubcategoryId = subcategory.Id;
                    }

                    var existingProduct = await _context.Products
                        .FirstOrDefaultAsync(p => p.Id == product.Id);

                    if (existingProduct == null)
                    {
                        _context.Products.Add(product);
                        _logger.LogInformation($"Added product: {product.Name}");
                    }
                    else
                    {
                        existingProduct.Name = product.Name;
                        existingProduct.Brand = product.Brand;
                        existingProduct.Price = product.Price;
                        existingProduct.Category = product.Category;
                        existingProduct.SubCategory = product.SubCategory;
                        existingProduct.CategoryId = product.CategoryId;
                        existingProduct.SubcategoryId = product.SubcategoryId;
                        existingProduct.Quantity = product.Quantity;
                        existingProduct.Warranty = product.Warranty;
                        existingProduct.Images = product.Images;

                        _context.Products.Update(existingProduct);
                        _logger.LogInformation($"Updated product: {product.Name}");
                    }
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Products seeding/updating completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding/updating products.");
                throw;
            }
        }

        public async Task SeedUsersAsync()
        {
            try
            {
                if (await _context.Users.AnyAsync()) return;

                var users = UserAbstractions.GetPredefinedUsers();
                await _context.Users.AddRangeAsync(users);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Seeded {users.Count} users.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding users.");
                throw;
            }
        }

        public async Task SeedOrUpdateUsersAsync()
        {
            try
            {
                var users = UserAbstractions.GetPredefinedUsers();

                foreach (var user in users)
                {
                    var existingUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email.ToLower() == user.Email.ToLower());

                    if (existingUser == null)
                    {
                        _context.Users.Add(user);
                        _logger.LogInformation($"Added user: {user.Email}");
                    }
                    else
                    {
                        existingUser.Name = user.Name;
                        existingUser.LastName = user.LastName;
                        existingUser.Email = user.Email;
                        existingUser.Address = user.Address;
                        existingUser.Role = user.Role;

                        _context.Users.Update(existingUser);
                        _logger.LogInformation($"Updated user: {user.Email}");
                    }
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Users seeding/updating completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding/updating users.");
                throw;
            }
        }

        public async Task SeedRatingsAsync()
        {
            try
            {
                if (await _context.Ratings.AnyAsync()) return;

                var ratings = RatingAbstractions.GetPredefinedRatings();
                await _context.Ratings.AddRangeAsync(ratings);
                await _context.SaveChangesAsync();

                await UpdateProductRatingStatistics();
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Seeded {ratings.Count} ratings and updated product stats.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding ratings.");
                throw;
            }
        }

        public async Task SeedOrUpdateRatingsAsync()
        {
            try
            {
                var ratings = RatingAbstractions.GetPredefinedRatings();

                foreach (var rating in ratings)
                {
                    var existingRating = await _context.Ratings
                        .FirstOrDefaultAsync(r => r.UserId == rating.UserId && r.ProductId == rating.ProductId);

                    if (existingRating == null)
                    {
                        _context.Ratings.Add(rating);
                        _logger.LogInformation($"Added rating: User {rating.UserId} -> Product {rating.ProductId} ({rating.Value})");
                    }
                    else
                    {
                        existingRating.Value = rating.Value;
                        existingRating.RatedAt = rating.RatedAt;
                        _context.Ratings.Update(existingRating);
                        _logger.LogInformation($"Updated rating: User {rating.UserId} -> Product {rating.ProductId}");
                    }
                }

                await _context.SaveChangesAsync();
                await UpdateProductRatingStatistics();
                await _context.SaveChangesAsync();

                _logger.LogInformation("Ratings seeding/updating completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding/updating ratings.");
                throw;
            }
        }

        private async Task UpdateProductRatingStatistics()
        {
            var ratingsByProduct = await _context.Ratings
                .GroupBy(r => r.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Count = g.Count(),
                    Sum = g.Sum(r => r.Value),
                    Average = g.Average(r => r.Value)
                })
                .ToListAsync();

            foreach (var stats in ratingsByProduct)
            {
                var product = await _context.Products.FindAsync(stats.ProductId);
                if (product != null)
                {
                    product.RatingCount = stats.Count;
                    product.RatingSum = stats.Sum;
                    product.AverageRating = Math.Round(stats.Average, 2);
                    _context.Products.Update(product);
                }
            }
        }
    }
}
