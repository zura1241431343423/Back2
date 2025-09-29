using E_commerce.Models;
using System.Security.Cryptography;
using System.Text;
namespace E_commerce.Abstractions
{
    public static class UserAbstractions
    {
        public static List<User> GetPredefinedUsers()
        {
            return new List<User>
            {
                new User
                {
                    Name = "Admin",
                    LastName = "User",
                    Email = "user1@gmail.com",
                    Password = HashPassword("12345678"),
                    Address = "123 Admin Street, Admin City",
                    Role = UserRole.Admin
                },
                new User
                {
                    Name = "Admin",
                    LastName = "Secondary",
                    Email = "user2@gmail.com",
                    Password = HashPassword("12345678"),
                    Address = "456 Admin Avenue, Admin City",
                    Role = UserRole.Admin
                },
                new User
                {
                    Name = "Super",
                    LastName = "Admin",
                    Email = "user3@gmail.com",
                    Password = HashPassword("12345678"),
                    Address = "789 Super Street, Admin City",
                    Role = UserRole.Admin
                },
                new User
                {
                    Name = "System",
                    LastName = "Admin",
                    Email = "user4@gmail.com",
                    Password = HashPassword("12345678"),
                    Address = "101 System Boulevard, Admin City",
                    Role = UserRole.Admin
                },
                new User
                {
                    Name = "Store",
                    LastName = "Manager",
                    Email = "user5@gmail.com",
                    Password = HashPassword("12345678"),
                    Address = "202 Manager Lane, Business District",
                    Role = UserRole.Manager
                },
                new User
                {
                    Name = "Test",
                    LastName = "User",
                    Email = "testuser@gmail.com",
                    Password = HashPassword("12345678"),
                    Address = "303 User Street, Residential Area",
                    Role = UserRole.User
                }
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}