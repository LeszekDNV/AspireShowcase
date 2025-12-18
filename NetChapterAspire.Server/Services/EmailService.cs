using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NetChapterAspire.Server.Services.Interfaces;

namespace NetChapterAspire.Server.Services;

public class EmailService : IEmailService
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _logger = logger;
        var (host, port) = ParseSmtpConnectionString(configuration.GetConnectionString("smtp"));
        _smtpHost = host;
        _smtpPort = port;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("NetChapter Aspire", "noreply@netchapter.com"));
        message.To.Add(new MailboxAddress("User", to));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlBody
        };

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpHost, _smtpPort, SecureSocketOptions.None);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        _logger.LogInformation("Email sent successfully to {To}", to);
    }

    private static (string host, int port) ParseSmtpConnectionString(string? connectionString)
    {
        string host = "localhost";
        int port = 1025;

        if (string.IsNullOrEmpty(connectionString))
            return (host, port);

        var parts = connectionString.Split(';');
        foreach (var part in parts)
        {
            var keyValue = part.Split('=', 2);
            if (keyValue.Length != 2) continue;

            var key = keyValue[0].Trim();
            var value = keyValue[1].Trim();

            if (key.Equals("host", StringComparison.OrdinalIgnoreCase))
                host = value;
            else if (key.Equals("port", StringComparison.OrdinalIgnoreCase))
                port = int.Parse(value);
        }

        return (host, port);
    }
}
