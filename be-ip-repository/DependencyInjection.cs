using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using be_ip_repository.Blob.Settings;
using be_ip_repository.Blob.Interface;
using be_ip_repository.Blob;
using be_ip_repository.Cosmos.Interfaces;
using be_ip_repository.Cosmos;
using be_ip_repository.Cosmos.Settings;
using be_ip_repository.ServiceBus.Settings;
using be_ip_repository.ServiceBus.Interface;
using be_ip_repository.ServiceBus;
namespace be_ip_repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlobSettings>(configuration.GetSection("BlobSettings"));
            services.Configure<CosmosSettings>(configuration.GetSection("CosmosSettings"));
            services.Configure<ServiceBusSettings>(configuration.GetSection("ServiceBusSettings"));

            services.AddSingleton<IServiceBusRepository, ServiceBusRepository>();
            services.AddSingleton<IBlobRepository, BlobRepository>();
            services.AddSingleton<ICosmosRepository, CosmosRepository>();

            return services;
        }
    }
}
