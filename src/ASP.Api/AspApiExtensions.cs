using ASP.Infrastructure;
using ASP.Infrastructure.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ASP.Api
{
    public static class AspApiExtensions
    {
        public static IConfigurationBuilder ConfigureSettings(this IConfigurationBuilder builder, IHostEnvironment environment, IConfiguration configuration)
        {
            configuration.BindConfig<AzureKeyVaultOptions>(out var config);

            builder
                .AddJsonFile(Path.Combine(
                    environment.ContentRootPath, "apisettings.json"),
                    optional: false)
                .AddJsonFile(Path.Combine(
                    environment.ContentRootPath, "apisettings.local.json"),
                    optional: true)
                .AddEnvironmentVariables()
                .ConfigureAzureKeyVault(config);

            return builder;
        }
    }
}
