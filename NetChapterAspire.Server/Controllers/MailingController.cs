using Microsoft.AspNetCore.Mvc;
using NetChapterAspire.Server.Models.Common;
using NetChapterAspire.Server.Models.DTOs;
using NetChapterAspire.Server.Services.Interfaces;

namespace NetChapterAspire.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailingController(IEmailService emailService) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> SendMail([FromBody] SendMailRequest? request = null)
    {
        var to = request?.To ?? "test@example.com";
        var subject = request?.Subject ?? "Test Email from NetChapter Aspire";
        var body = request?.Body ?? @"
            <h1>Hello from NetChapter Aspire!</h1>
            <p>This is a test email sent via MailPit SMTP server.</p>
            <p>If you're seeing this, the email integration is working correctly! ??</p>
        ";

        await emailService.SendEmailAsync(to, subject, body);

        return Ok(ApiResponse<object>.SuccessResponse(
            new
            {
                to,
                subject
            },
            "Email sent successfully!"
        ));
    }
}