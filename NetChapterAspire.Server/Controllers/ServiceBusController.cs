using Microsoft.AspNetCore.Mvc;
using NetChapterAspire.Server.Models.Common;
using NetChapterAspire.Server.Models.DTOs;
using NetChapterAspire.Server.Services.Interfaces;

namespace NetChapterAspire.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceBusController(IServiceBusService serviceBusService) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        if (string.IsNullOrEmpty(request.Message))
        {
            return BadRequest(ApiResponse.ErrorResponse("Message cannot be empty"));
        }

        await serviceBusService.SendMessageAsync(request.Message, request.Subject);

        return Ok(ApiResponse<object>.SuccessResponse(
            new
            {
                queueName = "demo-queue",
                subject = request.Subject ?? "Demo Message"
            },
            "Message sent successfully!"
        ));
    }

    [HttpGet("receive")]
    public async Task<IActionResult> ReceiveMessages([FromQuery] int maxMessages = 10)
    {
        var receivedMessages = await serviceBusService.ReceiveMessagesAsync(
            maxMessages, 
            TimeSpan.FromSeconds(5)
        );

        var messages = receivedMessages.Select(message => new
        {
            messageId = message.MessageId,
            subject = message.Subject,
            body = message.Body.ToString(),
            enqueuedTime = message.EnqueuedTime,
            deliveryCount = message.DeliveryCount,
            properties = message.ApplicationProperties
        }).ToList();

        // Complete all received messages
        foreach (var message in receivedMessages)
        {
            await serviceBusService.CompleteMessageAsync(message);
        }

        return Ok(ApiResponse<object>.SuccessResponse(
            new { count = messages.Count, messages }
        ));
    }

    [HttpGet("peek")]
    public async Task<IActionResult> PeekMessages([FromQuery] int maxMessages = 10)
    {
        var peekedMessages = await serviceBusService.PeekMessagesAsync(maxMessages);

        var messages = peekedMessages.Select(message => new
        {
            messageId = message.MessageId,
            subject = message.Subject,
            body = message.Body.ToString(),
            enqueuedTime = message.EnqueuedTime,
            deliveryCount = message.DeliveryCount,
            properties = message.ApplicationProperties
        }).ToList();

        return Ok(ApiResponse<object>.SuccessResponse(
            new { count = messages.Count, messages }
        ));
    }
}