using be_ip_repository.Cosmos.Entities;

namespace be_ip_repository.Cosmos.Interfaces
{
    public interface ICosmosRepository
    {
        Task<IList<ProductEntity>> GetProducts();

        Task<ProductEntity> GetProductById(string id);
    }
}
