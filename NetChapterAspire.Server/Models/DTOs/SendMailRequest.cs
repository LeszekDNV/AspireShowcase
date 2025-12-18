namespace NetChapterAspire.Server.Models.DTOs;

public record SendMailRequest
{
    public string? To { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
}