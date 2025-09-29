using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using E_commerce.Data;
using E_commerce.Emails;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(DataContext context, IEmailSender emailSender, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _context = context;
            _emailSender = emailSender;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model == null)
                return BadRequest("Invalid request payload");

            try
            {
                _logger.LogInformation($"Login attempt for email: {model.Email}");

                
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());
                if (user == null)
                {
                    _logger.LogWarning($"User not found for email: {model.Email}");
                    return Unauthorized("Invalid credentials");
                }

                var hashedPassword = HashPassword(model.Password);
                _logger.LogInformation($"Stored password hash: {user.Password}");
                _logger.LogInformation($"Provided password hash: {hashedPassword}");

                if (user.Password != hashedPassword)
                {
                    _logger.LogWarning($"Password mismatch for email: {model.Email}");
                    return Unauthorized("Invalid credentials");
                }

                var token = GenerateJwtToken(user);
                _logger.LogInformation($"User {user.Email} logged in successfully");

                return Ok(new
                {
                    token = token,
                    role = user.Role.ToString(),
                    userId = user.Id,
                    name = user.Name,
                    lastName = user.LastName
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Login error for email: {model?.Email}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());
            if (user == null)
                return Ok();

            var token = GenerateToken();
            var expiration = DateTime.UtcNow.AddHours(1);

            var resetToken = new PasswordResetToken
            {
                Email = model.Email,
                Token = token,
                Expiration = expiration
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            var resetLink = $"{_configuration["ClientUrl"]}/reset-password?token={token}&email={model.Email}";
            var message = $"Please reset your password by clicking <a href='{resetLink}'>here</a>";

            await _emailSender.SendEmailAsync(model.Email, "Password Reset", message);

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var token = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t =>
                    t.Token == model.Token &&
                    t.Email.ToLower() == model.Email.ToLower() &&
                    !t.IsUsed &&
                    t.Expiration > DateTime.UtcNow);

            if (token == null)
                return BadRequest("Invalid or expired token");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());
            if (user == null)
                return BadRequest("User not found");

            user.Password = HashPassword(model.NewPassword);
            token.IsUsed = true;

            await _context.SaveChangesAsync();

            return Ok();
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var tokenBytes = new byte[32];
            rng.GetBytes(tokenBytes);
            return Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    public class LoginModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        public string Token { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string NewPassword { get; set; }
    }
}