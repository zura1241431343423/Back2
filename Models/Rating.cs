using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace E_commerce.Models
{
   

    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Value { get; set; }

        [Required]
        public DateTime RatedAt { get; set; } = DateTime.UtcNow;

        
        [JsonIgnore]
        public virtual Product Product { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
