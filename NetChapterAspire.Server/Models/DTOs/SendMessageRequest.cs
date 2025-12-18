namespace NetChapterAspire.Server.Models.DTOs;

public record SendMessageRequest
{
    public required string Message { get; set; }
    public string? Subject { get; set; }
}