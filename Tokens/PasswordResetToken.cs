
using System;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models
{
    public class PasswordResetToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public bool IsUsed { get; set; } = false;
    }
}