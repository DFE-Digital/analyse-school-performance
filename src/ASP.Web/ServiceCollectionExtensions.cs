using ASP.Application;
using ASP.Infrastructure;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using AppEnvironmentVariables = ASP.Infrastructure.Constants.EnvironmentVariables;

namespace ASP.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApp(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                .AddRouting(options => {
                    options.LowercaseUrls = true;
                    options.AppendTrailingSlash = true;
                })
                .Configure<RazorViewEngineOptions>(options =>
                {
                    // {3} is the feature, {2} is the area, {1} is the controller, {0} is the action

                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Shared/{0}" + RazorViewEngine.ViewExtension);
                })
                .AddControllersWithViews();

            return services;
        }
        
        /// <summary>
        /// This method configures various aspects of the application's environment, including key vaults and settings files.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureAppEnvironment(this IServiceCollection services, ConfigurationManager configuration)
        {
            configuration.AddEnvironmentVariables();
            
            configuration
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", true);

            services.ConfigureAzureKeyVault(configuration);

            return services;
        }

        public static IServiceCollection ConfigureSearch(this IServiceCollection services)
        {
            services.ConfigureSearchStrategyFactory();
            services.ConfigureSearchServices();
            return services;
        }
        
        /// <summary>
        /// Configures the Azure Key Vault integration for the application.
        /// This method initializes the Key Vault client with the necessary credentials
        /// and configures the application to use secrets stored in the Azure Key Vault.
        /// It is typically called during the application's startup configuration process.
        /// See the following link for more info: https://learn.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-8.0
        /// </summary>
        /// <param name="services">The IServiceCollection instance, which is used to register application services and configure dependency injection.
        /// This parameter is extended by this method to include services related to the Azure Key Vault.</param>
        /// <param name="configuration">The ConfigurationManager instance, which provides access to application settings.
        /// It is used here to retrieve the name of the Key Vault from the application's configuration settings and
        /// to add the Key Vault configuration to the application's configuration system.</param>
        /// <returns>The IServiceCollection instance, which now includes services configured to interact with Azure Key Vault.
        /// This allows for chaining further configurations.</returns>
        /// <remarks>
        /// Ensure that the application's configuration includes the Key Vault name specified by 'AspAzureKeyVaultName'.
        /// This method checks if the Key Vault name is provided and is not empty; if so, it configures the Key Vault integration
        /// using the default Azure credential, which requires the application to be running in an environment
        /// where Azure AD authentication is available (e.g., Azure VM, Azure App Services).
        /// If the Key Vault name is not specified or is empty, the Key Vault integration will not be configured,
        /// and the services collection will be returned unchanged.
        /// This method should be called during the application startup, typically in the ConfigureServices method of the Startup class.
        /// </remarks>

        private static IServiceCollection ConfigureAzureKeyVault(this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var keyVaultName = configuration[AppEnvironmentVariables.AspAzureKeyVaultName];

            if (!string.IsNullOrEmpty(keyVaultName))
            {
                var credential = new DefaultAzureCredential();
                configuration.AddAzureKeyVault(new Uri($"https://{keyVaultName}.vault.azure.net/"), credential);
            }

            return services;
        }
    }
}