namespace ASP.Infrastructure.Azure.KeyVault
{
    public class AzureKeyVaultOptions
    {
        public const string SectionName = "AzureKeyVault";

        public string? KeyVaultName { get; set; }
    }
}
