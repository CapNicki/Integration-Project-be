using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using be_ip_common.Keyvault.Interface;
using be_ip_repository.Blob.Interface;
using be_ip_repository.Blob.Settings;
using Microsoft.Extensions.Options;

namespace be_ip_repository.Blob
{
    public class BlobRepository : IBlobRepository
    {
        private readonly BlobSettings _settings;
        private readonly string _storageAccountKey;


        private readonly BlobServiceClient _serviceClient;
        private readonly BlobContainerClient _containerClient;
        private readonly IKeyVaultService _keyVaultService;

        public BlobRepository(IKeyVaultService keyVaultService, IOptions<BlobSettings> blobSettings)
        {
            _settings = blobSettings.Value;

            _keyVaultService = keyVaultService;
            _storageAccountKey = _keyVaultService.GetSecret(_settings.BlobStorageKeyName);

            var credential = new DefaultAzureCredential();
            _serviceClient = new BlobServiceClient(new Uri(_settings.StorageAccountUrl), credential);
            _containerClient = _serviceClient.GetBlobContainerClient(_settings.ContainerName);
        }

        public string GetSasUrlForPicture(string pictureName)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(pictureName);

            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _settings.ContainerName,
                BlobName = pictureName,
                Resource = "b", 
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1) 
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            string sasToken = sasBuilder.ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(_settings.StorageAccountName, _storageAccountKey)).ToString();

            string sasUri = blobClient.Uri + "?" + sasToken;

            return sasUri;
        }
    }
}
