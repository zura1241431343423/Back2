namespace E_commerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } 
        public decimal TotalPrice { get; set; }
        public int TotalAmount { get; set; }
        public string DeliveryType { get; set; }
        public DateTime? DeliveryDate { get; set; } 
        public string Email { get; set; }
        public string Location { get; set; }
        public User? User { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}