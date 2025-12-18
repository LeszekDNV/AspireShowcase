using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetChapterAspire.Server.Data;
using NetChapterAspire.Server.Models;
using NetChapterAspire.Server.Models.DTOs;

namespace NetChapterAspire.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController(ApplicationDbContext context, ILogger<DatabaseController> logger) : ControllerBase
{
    [HttpGet("books")]
    public async Task<IActionResult> GetBooks()
    {
        try
        {
            List<Book> books = await context.Books
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return Ok(books);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving books");
            return StatusCode(500, new { message = $"Error retrieving books: {ex.Message}" });
        }
    }

    [HttpPost("books")]
    public async Task<IActionResult> AddBook([FromBody] AddBookRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            Book book = new()
            {
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                PageCount = request.PageCount,
                CreatedAt = DateTime.UtcNow
            };

            context.Books.Add(book);
            await context.SaveChangesAsync();

            logger.LogInformation("Book added successfully: {Title} by {Author}", book.Title, book.Author);

            return Ok(new
            {
                success = true,
                message = "Book added successfully!",
                book = book
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding book");
            return StatusCode(500, new
            {
                success = false,
                message = $"Error adding book: {ex.Message}"
            });
        }
    }

    [HttpDelete("books/{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        try
        {
            Book? book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            logger.LogInformation("Book deleted successfully: {Title}", book.Title);

            return Ok(new
            {
                success = true,
                message = "Book deleted successfully!"
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting book");
            return StatusCode(500, new
            {
                success = false,
                message = $"Error deleting book: {ex.Message}"
            });
        }
    }
}