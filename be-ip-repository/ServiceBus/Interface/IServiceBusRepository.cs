using Azure.Messaging.ServiceBus;

namespace be_ip_repository.ServiceBus.Interface
{
    public interface IServiceBusRepository
    {
        Task SendAsync(object o);

        Task<ServiceBusReceivedMessage> ReceiveAsync();

        Task CompleteMessageAsync(ServiceBusReceivedMessage message);
    }
}
