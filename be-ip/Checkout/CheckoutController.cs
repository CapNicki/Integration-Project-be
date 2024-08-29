using Azure.Identity;
using Azure.Messaging.ServiceBus;
using be_ip.Checkout.Classes;
using be_ip.Checkout.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace be_ip.Checkout
{
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private ServiceBusClient _sbClient;
        private ServiceBusSender _sbSender;
        private ServiceBusReceiver _sbReceiver;

        private readonly ServiceBusSettings _settings;

        public CheckoutController(IOptions<ServiceBusSettings> settings) 
        {
            _settings = settings.Value;

            var credentials = new DefaultAzureCredential();
            _sbClient = new ServiceBusClient(_settings.ServiceBusUrl, credentials);
            _sbSender = _sbClient.CreateSender(_settings.QueueName);
            _sbReceiver = _sbClient.CreateReceiver(_settings.QueueName);
        }

        [HttpPost("api/checkout")]
        public async Task<CheckoutPost> PostCheckout(CheckoutPost req)
        {
            await _sbSender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(req)));

            var receivedMessage = await _sbReceiver.ReceiveMessageAsync();

            await _sbReceiver.CompleteMessageAsync(receivedMessage);
            var receivedObject = receivedMessage.Body.ToObjectFromJson<CheckoutPost>();

            return receivedObject;
        }
    }
}
