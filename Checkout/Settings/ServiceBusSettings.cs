namespace be_ip.Checkout.Settings
{
    public class ServiceBusSettings
    {
        public required string ServiceBusUrl { get; set; }

        public required string QueueName { get; set; }
    }
}
