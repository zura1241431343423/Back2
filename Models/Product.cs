using System;
using System.Collections.Generic;

namespace E_commerce.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public int? DiscountPercentage { get; set; }
        public int Quantity { get; set; }
        public int Warranty { get; set; }
        public string[] Images { get; set; } = Array.Empty<string>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int RatingSum { get; set; } = 0;
        public int RatingCount { get; set; } = 0;
        public double AverageRating { get; set; } = 0;

       
        public string Category { get; set; } = string.Empty;
        public string SubCategory { get; set; } = string.Empty;

        
        public int? CategoryId { get; set; }
        public Category? CategoryEntity { get; set; }

        public int? SubcategoryId { get; set; }
        public Subcategory? SubcategoryEntity { get; set; }

        
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        
        public void ApplyDiscount(int discountPercentage)
        {
            if (discountPercentage >= 1 && discountPercentage <= 100)
            {
                DiscountPercentage = discountPercentage;
                DiscountedPrice = Price - (Price * discountPercentage / 100);
            }
        }

        public void RemoveDiscount()
        {
            DiscountPercentage = null;
            DiscountedPrice = null;
        }
    }
}
