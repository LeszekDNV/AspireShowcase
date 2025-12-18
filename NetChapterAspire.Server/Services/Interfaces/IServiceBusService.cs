using Azure.Messaging.ServiceBus;

namespace NetChapterAspire.Server.Services.Interfaces;

public interface IServiceBusService
{
    Task SendMessageAsync(string message, string? subject = null);
    Task<IReadOnlyList<ServiceBusReceivedMessage>> ReceiveMessagesAsync(int maxMessages, TimeSpan waitTime);
    Task<IReadOnlyList<ServiceBusReceivedMessage>> PeekMessagesAsync(int maxMessages);
    Task CompleteMessageAsync(ServiceBusReceivedMessage message);
}
