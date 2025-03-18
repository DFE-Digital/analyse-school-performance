using ASP.Infrastructure.Azure.CosmosDbSeeder.Configuration;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlobStorageServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<BlobStorageOptions>(configuration.GetSection("BlobStorage"));
        
        services.AddSingleton<IBlobStorageService>(sp =>
        {
            var config = sp.GetRequiredService<IOptions<BlobStorageOptions>>().Value;
            var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();
            return new BlobStorageService(config.ConnectionString, logger);
        });

        return services;
    }
}