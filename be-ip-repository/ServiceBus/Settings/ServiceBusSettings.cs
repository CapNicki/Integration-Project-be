namespace be_ip_repository.ServiceBus.Settings
{
    public class ServiceBusSettings
    {
        public required string ServiceBusUrl { get; set; }

        public required string QueueName { get; set; }
    }
}
