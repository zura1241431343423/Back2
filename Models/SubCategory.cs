namespace E_commerce.Models
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}