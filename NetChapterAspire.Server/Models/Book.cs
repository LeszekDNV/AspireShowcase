namespace NetChapterAspire.Server.Models;

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string ISBN { get; set; }
    public int PageCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}