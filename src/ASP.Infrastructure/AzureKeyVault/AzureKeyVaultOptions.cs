namespace ASP.Infrastructure.AzureKeyVault
{
    public class AzureKeyVaultOptions
    {
        public const string SectionName = "AzureKeyVault";

        public string? KeyVaultName { get; set; }
    }
}
