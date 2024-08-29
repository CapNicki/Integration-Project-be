using Azure.Identity;
using be_ip_repository.Cosmos.Entities;
using be_ip_repository.Cosmos.Interfaces;
using be_ip_repository.Cosmos.Settings;
using be_ip_repository.Helpers;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;

namespace be_ip_repository.Cosmos
{
    public class CosmosRepository : ICosmosRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;
        private readonly CosmosSettings _settings;

        public CosmosRepository(IOptions<CosmosSettings> settings) 
        {
            var credential = new DefaultAzureCredential();

            _settings = settings.Value;
            _cosmosClient = new CosmosClient(_settings.AccountEndpoint, credential);

            _cosmosClient.CreateDatabaseIfNotExistsAsync(_settings.DatabaseName);
            var db = _cosmosClient.GetDatabase(_settings.DatabaseName);

            db.CreateContainerIfNotExistsAsync(_settings.ContainerName, "/id");
            _container = db.GetContainer(_settings.ContainerName);
        }

        public async Task<ProductEntity> GetProductById(string id)
        {
            ItemResponse<ProductEntity> response = await _container.ReadItemAsync<ProductEntity>(id, new PartitionKey(id));
            return response.Resource;
        }

        public async Task<IList<ProductEntity>> GetProducts()
        {
            return await _container.GetItemLinqQueryable<ProductEntity>().ToFeedIterator().ToListAsync();
        }
    }
}
