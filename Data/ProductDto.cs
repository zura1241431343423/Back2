namespace E_commerce.DTOs
{
    public class ProductCreateUpdateDto
    {
        public string Name { get; set; }
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Warranty { get; set; }
        public string[] Images { get; set; }

        
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }
}
