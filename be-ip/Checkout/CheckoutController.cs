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
        public async Task PostCheckout(CheckoutPost req)
        {
            await _serviceBusRepository.SendAsync(req);
        }
    }
}
