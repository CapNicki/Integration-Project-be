using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using be_ip_common.Keyvault.Interface;
using be_ip_common.Keyvault.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace be_ip_common.Keyvault
{
    public class KeyVaultService : IKeyVaultService
    {
        private readonly SecretClient _secretClient;
        private readonly ConcurrentDictionary<string, string> _cache;
        private readonly KeyVaultSettings _settings;

        public KeyVaultService(IOptions<KeyVaultSettings> keyVaultSettings)
        {
            _settings = keyVaultSettings.Value;

            var credential = new DefaultAzureCredential();
            _secretClient = new SecretClient(new Uri(_settings.KeyVaultUrl), credential);
            _cache = new ConcurrentDictionary<string, string>();
        }

        public string GetSecret(string secretName)
        {
            return GetSecretAsync(secretName).GetAwaiter().GetResult();
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            if (_cache.TryGetValue(secretName, out string cachedSecret))
            {
                return cachedSecret;
            }

            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);

            _cache.TryAdd(secretName, secret.Value);

            return secret.Value;
        }
    }
}
