namespace NetChapterAspire.Server.Models.DTOs;

public record AddBookRequest
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string ISBN { get; set; }
    public int PageCount { get; set; }
}