using ASP.Core.Configuration;
using ASP.Infrastructure.Azure.KeyVault;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

        public static IServiceCollection AddOpenApiConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
            {
                return new OpenApiConfigurationOptions()
                {
                    Info = new OpenApiInfo()
                    {
                        Version = DefaultOpenApiConfigurationOptions.GetOpenApiDocVersion(),
                        Title = DefaultOpenApiConfigurationOptions.GetOpenApiDocTitle(),
                        Description = DefaultOpenApiConfigurationOptions.GetOpenApiDocDescription(),
                        Contact = new OpenApiContact
                        {
                            Name = "Analyse School Performance",
                            Email = "asp-contact-info@gov.com",
                            Url = new Uri("https://gov.uk"),
                        }
                    },
                    Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                    ForceHttps = DefaultOpenApiConfigurationOptions.IsHttpsForced(),
                    ForceHttp = DefaultOpenApiConfigurationOptions.IsHttpForced(),
                };
            });

            return services;
        }
    }
}
