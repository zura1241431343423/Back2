using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required] public string Name { get; set; }
        [Required] public string LastName { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Required, DataType(DataType.Password)] public string Password { get; set; }
        public string Address { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    }
}
