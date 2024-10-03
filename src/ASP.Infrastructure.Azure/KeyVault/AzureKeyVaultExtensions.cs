using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace ASP.Infrastructure.Azure.KeyVault
{
    public static class AzureKeyVaultExtensions
    {
        /// <summary>
        /// Configures the Azure Key Vault integration for the application.
        /// This method initializes the Key Vault client with the necessary credentials
        /// and configures the application to use secrets stored in the Azure Key Vault.
        /// It is typically called during the application's startup configuration process.
        /// See the following link for more info: https://learn.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-8.0
        /// </summary>
        /// <param name="builder">The IConfigurationBuilder instance, which is used to register configuration sources.
        /// This parameter is extended by this method to include configuration related to the Azure Key Vault.</param>
        /// <param name="config">An AzureKeyVaultOptions instance, which contains the name of the Key Vault.</param>
        /// <returns>The IConfigurationBuilder instance, which now includes services configured to interact with Azure Key Vault.
        /// This allows for chaining further configurations.</returns>
        /// <remarks>
        /// Ensure that the application's configuration includes the Key Vault name specified by 'AzureKeyVault:KeyVaultName'.
        /// This method checks if the Key Vault name is provided and is not empty; if so, it configures the Key Vault integration
        /// using the default Azure credential, which requires the application to be running in an environment
        /// where Azure AD authentication is available (e.g., Azure VM, Azure App Services).
        /// If the Key Vault name is not specified or is empty, the Key Vault integration will not be configured,
        /// and the services collection will be returned unchanged.
        /// This method should be called during the application startup, typically in the ConfigureServices method of the Startup class.
        /// </remarks>
        public static IConfigurationBuilder ConfigureAzureKeyVault(this IConfigurationBuilder builder, AzureKeyVaultOptions config)
        {
            if (!string.IsNullOrEmpty(config.KeyVaultName))
            {
                var credential = new DefaultAzureCredential();
                builder.AddAzureKeyVault(new Uri($"https://{config.KeyVaultName}.vault.azure.net/"), credential);
            }

            return builder;
        }
    }
}
