using Microsoft.AspNetCore.Mvc;
using E_commerce.Models;
using E_commerce.Data;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly DataContext _context;

        public CommentController(DataContext context)
        {
            _context = context;
        }

       
        [HttpPost]
        public async Task<ActionResult<CommentResponseDto>> PostComment(Comment comment)
        {
            try
            {
                if (comment.UserId <= 0)
                    return BadRequest("Valid UserId is required.");

                if (comment.ProductId <= 0)
                    return BadRequest("Valid ProductId is required.");

                if (string.IsNullOrWhiteSpace(comment.Content))
                    return BadRequest("Comment content cannot be empty.");

                var user = await _context.Users.FindAsync(comment.UserId);
                if (user == null)
                    return BadRequest("User does not exist.");

                var productExists = await _context.Products.AnyAsync(p => p.Id == comment.ProductId);
                if (!productExists)
                    return BadRequest("Product does not exist.");

                comment.AddedAt = DateTime.UtcNow;
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                var responseDto = new CommentResponseDto
                {
                    Id = comment.Id,
                    UserId = user.Id,
                    UserFullName = $"{user.Name} {user.LastName}",
                    ProductId = comment.ProductId,
                    Content = comment.Content,
                    Rating = comment.Rating,
                    AddedAt = comment.AddedAt,
                    LastModified = comment.LastModified
                };

                return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetComments()
        {
            try
            {
                var comments = await _context.Comments
                    .Include(c => c.User)
                    .OrderByDescending(c => c.AddedAt)
                    .Select(c => new CommentResponseDto
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        UserFullName = $"{c.User.Name} {c.User.LastName}",
                        ProductId = c.ProductId,
                        Content = c.Content,
                        Rating = c.Rating,
                        AddedAt = c.AddedAt,
                        LastModified = c.LastModified
                    })
                    .ToListAsync();

                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

       
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetCommentsByProductId(int productId)
        {
            try
            {
                var comments = await _context.Comments
                    .Include(c => c.User)
                    .Where(c => c.ProductId == productId)
                    .OrderByDescending(c => c.AddedAt)
                    .Select(c => new CommentResponseDto
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        UserFullName = $"{c.User.Name} {c.User.LastName}",
                        ProductId = c.ProductId,
                        Content = c.Content,
                        Rating = c.Rating,
                        AddedAt = c.AddedAt,
                        LastModified = c.LastModified
                    })
                    .ToListAsync();

                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

       
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetCommentsByUserId(int userId)
        {
            try
            {
                var comments = await _context.Comments
                    .Include(c => c.User)
                    .Where(c => c.UserId == userId)
                    .OrderByDescending(c => c.AddedAt)
                    .Select(c => new CommentResponseDto
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        UserFullName = $"{c.User.Name} {c.User.LastName}",
                        ProductId = c.ProductId,
                        Content = c.Content,
                        Rating = c.Rating,
                        AddedAt = c.AddedAt,
                        LastModified = c.LastModified
                    })
                    .ToListAsync();

                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentResponseDto>> GetComment(int id)
        {
            try
            {
                var comment = await _context.Comments
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (comment == null)
                    return NotFound();

                var response = new CommentResponseDto
                {
                    Id = comment.Id,
                    UserId = comment.UserId,
                    UserFullName = $"{comment.User.Name} {comment.User.LastName}",
                    ProductId = comment.ProductId,
                    Content = comment.Content,
                    Rating = comment.Rating,
                    AddedAt = comment.AddedAt,
                    LastModified = comment.LastModified
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment updatedComment)
        {
            try
            {
                if (id != updatedComment.Id)
                    return BadRequest("Comment ID mismatch.");

                var existingComment = await _context.Comments.FindAsync(id);
                if (existingComment == null)
                    return NotFound();

                existingComment.Content = updatedComment.Content;
                existingComment.LastModified = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var comment = await _context.Comments.FindAsync(id);
                if (comment == null)
                    return NotFound();

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
