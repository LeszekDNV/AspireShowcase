using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetChapterAspire.Server.Data;
using NetChapterAspire.Server.Models;
using NetChapterAspire.Server.Models.Common;
using NetChapterAspire.Server.Models.DTOs;

namespace NetChapterAspire.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController(ApplicationDbContext context, ILogger<DatabaseController> logger) : ControllerBase
{
    [HttpGet("books")]
    public async Task<IActionResult> GetBooks()
    {
        var books = await context.Books
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return Ok(ApiResponse<IEnumerable<Book>>.SuccessResponse(books));
    }

    [HttpPost("books")]
    public async Task<IActionResult> AddBook([FromBody] AddBookRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse.ErrorResponse("Invalid model state"));
        }

        var book = new Book
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

        return Ok(ApiResponse<Book>.SuccessResponse(book, "Book added successfully!"));
    }

    [HttpDelete("books/{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound(ApiResponse.ErrorResponse("Book not found"));
        }

        context.Books.Remove(book);
        await context.SaveChangesAsync();

        logger.LogInformation("Book deleted successfully: {Title}", book.Title);

        return Ok(ApiResponse.SuccessResponse("Book deleted successfully!"));
    }
}