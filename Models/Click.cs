using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    public class Click
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

      
        public string? SubCategory { get; set; }

       
        public DateTime ClickedAt { get; set; } = DateTime.UtcNow;
    }
}