using ASP.Core.Configuration;
using ASP.Infrastructure.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Infrastructure.Azure.Blob
{
    public static class BlobStorageExtensions
    {
        public static IServiceCollection ConfigureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            configuration.BindConfig<BlobStorageOptions>(out var config);

            if (config.InMemory)
            {
                Infrastructure.Blob.BlobStorageExtensions.ConfigureBlobStorage(services, configuration);
            }
            else
            {
                ConfigureAzure(services, configuration);
            }

            return services;
        }

        private static void ConfigureAzure(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<AzureBlobStorageOptions>(configuration);

            services.RemoveAll<IBlobStorage>();
            services.TryAddScoped<IBlobStorage, AzureBlobStorage>();
        }
    }
}