using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly DataContext _context;

    public FavoritesController(DataContext context)
    {
        _context = context;
    }


    [HttpPost]
    public async Task<IActionResult> AddToFavorites([FromBody] FavoriteDto dto)
    {
        if (await _context.Favorites.AnyAsync(f => f.UserId == dto.UserId && f.ProductId == dto.ProductId))
            return Conflict("Already in favorites.");

        var favorite = new Favorite
        {
            UserId = dto.UserId,
            ProductId = dto.ProductId
        };

        _context.Favorites.Add(favorite);
        await _context.SaveChangesAsync();

        return Ok(favorite);
    }

    [HttpDelete("{userId}/{productId}")]
    public async Task<IActionResult> RemoveFromFavorites(int userId, int productId)
    {
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

        if (favorite == null)
            return NotFound();

        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();

        return NoContent();
    }

  
    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<int>>> GetUserFavorites(int userId)
    {
        var productIds = await _context.Favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.ProductId)
            .ToListAsync();

        return Ok(productIds);
    }
}
