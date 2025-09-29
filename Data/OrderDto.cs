using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Dtos
{
    public class OrderDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than 0.")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total amount must be at least 1.")]
        public int TotalAmount { get; set; }

        [Required]
        public string DeliveryType { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Location { get; set; }

        public DateTime? NominatedDate { get; set; } 

        [Required]
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
    }
}
