﻿namespace E_commerce.Models
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Role { get; set; } = "User";
    }
}
