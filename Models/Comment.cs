using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;

        
        [Range(1, 5)]
        public int? Rating { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        
        public DateTime? LastModified { get; set; }

        
        public User? User { get; set; }
        public Product? Product { get; set; }
    }

}
