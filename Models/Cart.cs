namespace E_commerce.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? Image { get; set; }

        
        public int QuantityAvailable { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
