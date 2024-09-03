using be_ip.Checkout.Classes;
using be_ip_repository.ServiceBus.Interface;
using Microsoft.AspNetCore.Mvc;

namespace be_ip.Checkout
{
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IServiceBusRepository _serviceBusRepository;

        public CheckoutController(IServiceBusRepository serviceBusRepository) 
        {
            _serviceBusRepository = serviceBusRepository;
        }

        [HttpPost("api/checkout")]
        public async Task<CheckoutPost> PostCheckout(CheckoutPost req)
        {
            await _serviceBusRepository.SendAsync(req);

            var message = await _serviceBusRepository.ReceiveAsync();

            var messageObject = message.Body.ToObjectFromJson<CheckoutPost>();

            await _serviceBusRepository.CompleteMessageAsync(message);

            return messageObject;
        }
    }
}
