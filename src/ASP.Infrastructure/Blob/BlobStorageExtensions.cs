using ASP.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ASP.Infrastructure.InMemory;

namespace ASP.Infrastructure.Blob
{
    public static class BlobStorageExtensions
    {
        private static readonly MemoryStore<string> _store = new();

        public static IServiceCollection ConfigureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<BlobStorageOptions>(configuration);

            services.TryAdd(new ServiceDescriptor(typeof(MemoryStore<string>), _store));
            services.TryAddSingleton<IBlobStorage, InMemoryBlobStorage>();

            return services;
        }
    }
}