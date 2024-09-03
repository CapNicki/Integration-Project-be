using be_ip_common.Keyvault;
using be_ip_common.Keyvault.Interface;
using be_ip_common.Keyvault.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace be_ip_common
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectCommon(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KeyVaultSettings>(configuration.GetSection("KeyVaultSettings"));

            services.AddSingleton<IKeyVaultService, KeyVaultService>();



            return services;
        }
    }
}
