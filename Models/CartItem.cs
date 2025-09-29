namespace E_commerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int QuantityAvailable { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

     
        public decimal TotalPrice => Price * Quantity;
    }
}
