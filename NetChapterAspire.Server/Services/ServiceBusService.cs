using Azure.Messaging.ServiceBus;
using NetChapterAspire.Server.Services.Interfaces;

namespace NetChapterAspire.Server.Services;

public class ServiceBusService : IServiceBusService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ILogger<ServiceBusService> _logger;
    private const string QueueName = "demo-queue";

    public ServiceBusService(ServiceBusClient serviceBusClient, ILogger<ServiceBusService> logger)
    {
        _serviceBusClient = serviceBusClient;
        _logger = logger;
    }

    public async Task SendMessageAsync(string message, string? subject = null)
    {
        var sender = _serviceBusClient.CreateSender(QueueName);

        var serviceBusMessage = new ServiceBusMessage(message)
        {
            Subject = subject ?? "Demo Message",
            MessageId = Guid.NewGuid().ToString(),
            ContentType = "text/plain"
        };

        serviceBusMessage.ApplicationProperties.Add("SentAt", DateTime.UtcNow);
        serviceBusMessage.ApplicationProperties.Add("Source", "NetChapter Aspire");

        await sender.SendMessageAsync(serviceBusMessage);

        _logger.LogInformation("Message sent to queue {QueueName}: {MessageId}", QueueName, serviceBusMessage.MessageId);
    }

    public async Task<IReadOnlyList<ServiceBusReceivedMessage>> ReceiveMessagesAsync(int maxMessages, TimeSpan waitTime)
    {
        var receiver = _serviceBusClient.CreateReceiver(QueueName);
        var messages = await receiver.ReceiveMessagesAsync(maxMessages, waitTime);
        
        _logger.LogInformation("Received {Count} messages from queue {QueueName}", messages.Count, QueueName);
        
        return messages;
    }

    public async Task<IReadOnlyList<ServiceBusReceivedMessage>> PeekMessagesAsync(int maxMessages)
    {
        var receiver = _serviceBusClient.CreateReceiver(QueueName);
        var messages = await receiver.PeekMessagesAsync(maxMessages);
        
        _logger.LogInformation("Peeked {Count} messages from queue {QueueName}", messages.Count, QueueName);
        
        return messages;
    }

    public async Task CompleteMessageAsync(ServiceBusReceivedMessage message)
    {
        var receiver = _serviceBusClient.CreateReceiver(QueueName);
        await receiver.CompleteMessageAsync(message);
    }
}
