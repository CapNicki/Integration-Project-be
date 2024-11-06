using be_ip.Product.Classes;
using be_ip_repository.Blob.Interface;
using be_ip_repository.Cosmos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace be_ip.Product
{
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ICosmosRepository _cosmosRepository;
        private readonly IBlobRepository _blobRepository;

        public ProductController(ICosmosRepository cosmosrepository, IBlobRepository blobRepository)
        {
            _cosmosRepository = cosmosrepository;
            _blobRepository = blobRepository;
        }

        [HttpGet("api/products")]
        public async Task<IList<BaseProduct>> GetProducts()
        {
            var productEntities = await _cosmosRepository.GetProducts();

            return productEntities.Select(pe => new BaseProduct()
            {
                Id = pe.id,
                Description = pe.Description,
                Price = pe.Price,
                Name = pe.Name,
                PictureUrl = _blobRepository.GetSasUrlForPicture("phoneish.webp")
            }).ToList(); 
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("api/products/{id}")]
        public async Task<BaseProduct> GetProduct(string id)
        {
            var productEntity = await _cosmosRepository.GetProductById(id);

            return new BaseProduct()
            {
                Id = productEntity.id,
                Description = productEntity.Description,
                Price = productEntity.Price,
                Name = productEntity.Name,
                PictureUrl = _blobRepository.GetSasUrlForPicture("phoneish.webp")
            };
        }
    }   
}
