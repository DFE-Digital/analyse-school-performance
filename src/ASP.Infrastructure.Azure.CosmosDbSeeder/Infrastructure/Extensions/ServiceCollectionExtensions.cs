using ASP.Infrastructure.Azure.CosmosDbSeeder.Configuration;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Services;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCosmosDbServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure options
        services.Configure<SourceCosmosDbOptions>(
            configuration.GetSection("CosmosDb:SourceDatabase"));
        
        services.Configure<TargetCosmosDbOptions>(
            configuration.GetSection("CosmosDb:TargetDatabase"));

        // Register database configuration
        var databaseConfig = new DatabaseConfig();
        configuration.GetSection("CosmosDb").Bind(databaseConfig);
        services.AddSingleton(databaseConfig);
        
        // Bind the Extraction section independently
        var extractionConfig = new ExtractionConfig();
        configuration.GetSection("Extraction").Bind(extractionConfig);
        services.AddSingleton(extractionConfig);

        // Register document processor
        services.AddScoped<IJsonDocumentProcessor, JsonDocumentProcessor>();

        // Register factory
        services.AddSingleton<ICosmosDbServiceFactory, CosmosDbServiceFactory>();

        return services;
    }
}