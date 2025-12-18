using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using NetChapterAspire.Server.Models.DTOs;

namespace NetChapterAspire.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailingController(IConfiguration configuration, ILogger<MailingController> logger) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> SendMail([FromBody] SendMailRequest? request = null)
    {
        try
        {
            // Get SMTP configuration from Aspire
            // Aspire passes MailPit config as ConnectionStrings:smtp
            string? smtpConnectionString = configuration.GetConnectionString("smtp");
                
            // Parse connection string or use defaults
            string smtpHost = "localhost";
            int smtpPort = 1025;

            if (!string.IsNullOrEmpty(smtpConnectionString))
            {
                // Connection string format: "host=localhost;port=1025"
                string[] parts = smtpConnectionString.Split(';');
                foreach (string part in parts)
                {
                    string[] keyValue = part.Split('=');
                    if (keyValue.Length == 2)
                    {
                        if (keyValue[0].Trim().Equals("host", StringComparison.OrdinalIgnoreCase))
                            smtpHost = keyValue[1].Trim();
                        else if (keyValue[0].Trim().Equals("port", StringComparison.OrdinalIgnoreCase))
                            smtpPort = int.Parse(keyValue[1].Trim());
                    }
                }
            }

            MimeMessage message = new();
            message.From.Add(new MailboxAddress("NetChapter Aspire", "noreply@netchapter.com"));
            message.To.Add(new MailboxAddress("Test User", request?.To ?? "test@example.com"));
            message.Subject = request?.Subject ?? "Test Email from NetChapter Aspire";

            BodyBuilder bodyBuilder = new()
            {
                HtmlBody = request?.Body ?? @"
                        <h1>Hello from NetChapter Aspire!</h1>
                        <p>This is a test email sent via MailPit SMTP server.</p>
                        <p>If you're seeing this, the email integration is working correctly! ??</p>
                    "
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (SmtpClient client = new())
            {
                // Connect to MailPit SMTP server (no SSL for local development)
                await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.None);

                // MailPit doesn't require authentication
                // await client.AuthenticateAsync("username", "password");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            logger.LogInformation("Email sent successfully to {To}", message.To);

            return Ok(new
            {
                success = true,
                message = "Email sent successfully!",
                details = new
                {
                    to = message.To.ToString(),
                    subject = message.Subject,
                    smtpHost = smtpHost,
                    smtpPort = smtpPort
                }
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending email");
            return StatusCode(500, new
            {
                success = false,
                message = $"Error sending email: {ex.Message}"
            });
        }
    }
}