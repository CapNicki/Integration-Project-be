using Azure.Identity;
using Azure.Messaging.ServiceBus;
using be_ip_repository.ServiceBus.Interface;
using be_ip_repository.ServiceBus.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace be_ip_repository.ServiceBus
{
    public class ServiceBusRepository : IServiceBusRepository
    {
        private readonly ServiceBusClient _sbClient;
        private readonly ServiceBusSender _sbSender;
        private readonly ServiceBusReceiver _sbReceiver;

        private readonly ServiceBusSettings _settings;

        public ServiceBusRepository(IOptions<ServiceBusSettings> settings)
        {
            _settings = settings.Value;

            var credentials = new DefaultAzureCredential();
            _sbClient = new ServiceBusClient(_settings.ServiceBusUrl, credentials);
            _sbSender = _sbClient.CreateSender(_settings.QueueName);
            _sbReceiver = _sbClient.CreateReceiver(_settings.QueueName);
        }

        public async Task SendAsync(object o)
        {
            await _sbSender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(o)));
        }

        public async Task<ServiceBusReceivedMessage> ReceiveAsync()
        {
            return await _sbReceiver.ReceiveMessageAsync();
        }

        public async Task CompleteMessageAsync(ServiceBusReceivedMessage message)
        {
            await _sbReceiver.CompleteMessageAsync(message);

        }
    }
}
