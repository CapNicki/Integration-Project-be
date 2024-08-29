namespace be_ip_repository.Cosmos.Settings
{
    public class CosmosSettings
    {
        public required string AccountEndpoint { get; set; }

        public required string DatabaseName { get; set; }

        public required string ContainerName { get; set; }
    }
}
