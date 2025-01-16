using ASP.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ASP.Infrastructure.InMemory;
using ASP.Infrastructure.Azure.Blob;
using ASP.Core.DataDownloads;

namespace ASP.Infrastructure.Blob
{
    public static class BlobStorageExtensions
    {
        private static readonly MemoryStore<string> _store = new();

        public static IServiceCollection ConfigureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            configuration.BindConfig<BlobStorageOptions>(out var config);

            if (config.InMemory)
            {
                ConfigureInMemory(services, configuration);
            }
            else
            {
                ConfigureAzure(services, configuration);
            }

            return services;
        }

        private static void ConfigureInMemory(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<BlobStorageOptions>(configuration);
            services.ConfigureOptions<DataDownloadsOptions>(configuration);

            services.RemoveAll<IBlobStorage>();
            services.TryAdd(new ServiceDescriptor(typeof(MemoryStore<string>), _store));
            services.TryAddScoped<IBlobStorage, InMemoryBlobStorage>();
        }

        private static void ConfigureAzure(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<AzureBlobStorageOptions>(configuration);

            services.RemoveAll<IBlobStorage>();
            services.TryAddScoped<IBlobStorage, AzureBlobStorage>();
        }
    }

    public static class DataDownloadsExtensions
    {
        public static IServiceCollection ConfigureDataDownloads(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<DataDownloadsOptions>(configuration);
            services.TryAddScoped<IDataDownloadsScopeValidator, DataDownloadsScope.Validator>();

            return services;
        }
    }
}