namespace E_commerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

      
        public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
