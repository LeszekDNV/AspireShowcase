using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using NetChapterAspire.Server.Models.DTOs;

namespace NetChapterAspire.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceBusController(ServiceBusClient serviceBusClient, ILogger<ServiceBusController> logger) : ControllerBase
{
    private const string QueueName = "demo-queue";

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        if (string.IsNullOrEmpty(request.Message))
        {
            return BadRequest(new { success = false, message = "Message cannot be empty" });
        }

        try
        {
            // Create a sender for the queue
            ServiceBusSender? sender = serviceBusClient.CreateSender(QueueName);

            // Create a message
            ServiceBusMessage message = new(request.Message)
            {
                Subject = request.Subject ?? "Demo Message",
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "text/plain"
            };

            // Add custom properties
            message.ApplicationProperties.Add("SentAt", DateTime.UtcNow);
            message.ApplicationProperties.Add("Source", "NetChapter Aspire");

            // Send the message
            await sender.SendMessageAsync(message);

            logger.LogInformation("Message sent to queue {QueueName}: {MessageId}", QueueName, message.MessageId);

            return Ok(new
            {
                success = true,
                message = "Message sent successfully!",
                details = new
                {
                    messageId = message.MessageId,
                    queueName = QueueName,
                    subject = message.Subject,
                    sentAt = DateTime.UtcNow
                }
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending message to Service Bus");
            return StatusCode(500, new
            {
                success = false,
                message = $"Error sending message: {ex.Message}"
            });
        }
    }

    [HttpGet("receive")]
    public async Task<IActionResult> ReceiveMessages([FromQuery] int maxMessages = 10)
    {
        try
        {
            // Create a receiver for the queue
            ServiceBusReceiver? receiver = serviceBusClient.CreateReceiver(QueueName);

            List<object> messages = [];
            IReadOnlyList<ServiceBusReceivedMessage>? receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages, TimeSpan.FromSeconds(5));

            foreach (ServiceBusReceivedMessage message in receivedMessages)
            {
                messages.Add(new
                {
                    messageId = message.MessageId,
                    subject = message.Subject,
                    body = message.Body.ToString(),
                    enqueuedTime = message.EnqueuedTime,
                    deliveryCount = message.DeliveryCount,
                    properties = message.ApplicationProperties
                });

                // Complete the message (remove from queue)
                await receiver.CompleteMessageAsync(message);
            }

            logger.LogInformation("Received {Count} messages from queue {QueueName}", messages.Count, QueueName);

            return Ok(new
            {
                success = true,
                count = messages.Count,
                messages = messages
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error receiving messages from Service Bus");
            return StatusCode(500, new
            {
                success = false,
                message = $"Error receiving messages: {ex.Message}"
            });
        }
    }

    [HttpGet("peek")]
    public async Task<IActionResult> PeekMessages([FromQuery] int maxMessages = 10)
    {
        try
        {
            // Create a receiver for the queue
            ServiceBusReceiver? receiver = serviceBusClient.CreateReceiver(QueueName);

            List<object> messages = [];
            IReadOnlyList<ServiceBusReceivedMessage>? peekedMessages = await receiver.PeekMessagesAsync(maxMessages);

            foreach (ServiceBusReceivedMessage message in peekedMessages)
            {
                messages.Add(new
                {
                    messageId = message.MessageId,
                    subject = message.Subject,
                    body = message.Body.ToString(),
                    enqueuedTime = message.EnqueuedTime,
                    deliveryCount = message.DeliveryCount,
                    properties = message.ApplicationProperties
                });
            }

            logger.LogInformation("Peeked {Count} messages from queue {QueueName}", messages.Count, QueueName);

            return Ok(new
            {
                success = true,
                count = messages.Count,
                messages = messages
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error peeking messages from Service Bus");
            return StatusCode(500, new
            {
                success = false,
                message = $"Error peeking messages: {ex.Message}"
            });
        }
    }
}