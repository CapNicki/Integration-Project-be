namespace be_ip_common.Keyvault.Interface
{
    public interface IKeyVaultService
    {
        Task<string> GetSecretAsync(string secretName);

        string GetSecret(string secretName);
    }
}
