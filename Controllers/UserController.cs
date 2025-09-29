using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(DataContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict("Email already registered.");

            try
            {
               
                if (!Enum.TryParse<UserRole>(dto.Role, true, out var parsedRole))
                    return BadRequest("Invalid role. Use Admin, Manager, or User.");

                var user = new User
                {
                    Name = dto.Name,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Password = HashPassword(dto.Password),
                    Address = dto.Address,
                    Role = parsedRole,
                    
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    user.Id,
                    user.Name,
                    user.LastName,
                    user.Email,
                    user.Role
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.LastName,
                    u.Email,
                    u.Role
                })
                .ToListAsync();

            return Ok(users);
        }

     
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.LastName,
                    u.Email,
                    u.Role,
                    u.Address
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProfileDto updatedUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound();

            existingUser.Name = updatedUser.Name;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Address = updatedUser.Address;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
