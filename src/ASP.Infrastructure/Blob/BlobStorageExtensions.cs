using ASP.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ASP.Infrastructure.InMemory;
using ASP.Infrastructure.Azure.Blob;

namespace ASP.Infrastructure.Blob
{
    public static class BlobStorageExtensions
    {
        private static readonly MemoryStore<string> _store = new();

        public static IServiceCollection ConfigureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<BlobStorageOptions>(configuration, out var config);

            if (config.InMemory)
            {
                ConfigureInMemory(services);
            }
            else
            {
                ConfigureAzure(services);
            }

            return services;
        }

        private static void ConfigureInMemory(IServiceCollection services)
        {
            services.RemoveAll<IBlobStorage>();
            services.TryAdd(new ServiceDescriptor(typeof(MemoryStore<string>), _store));
            services.TryAddScoped<IBlobStorage, InMemoryBlobStorage>();
        }

        private static void ConfigureAzure(IServiceCollection services)
        {
            services.RemoveAll<IBlobStorage>();
            services.TryAddScoped<IBlobStorage, AzureBlobStorage>();
        }
    }
}