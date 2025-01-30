using ASP.Core.Configuration;
using ASP.Infrastructure.DocumentDatabase;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Handlers.Query;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Infrastructure.Azure.CosmosDb;

public static class CosmosDbDocumentDatabaseExtensions
{
    public static IServiceCollection ConfigureDocumentDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<DocumentDatabaseOptions>(configuration, out var config);

        if (config.InMemory)
        {
            DocumentDatabaseExtensions.ConfigureDocumentDatabase(services, configuration);
        }
        else
        {
            ConfigureCosmos(services, configuration);
        }

        return services;
    }

    private static void ConfigureCosmos(IServiceCollection services, IConfiguration configuration)
    {
        services.RemoveAll<IDocumentDatabase>();

        services.AddCosmosDbDependencies(configuration);

        services.TryAddSingleton<IDocumentDatabase, CosmosDbDocumentDatabase>();
        services.TryAddSingleton<ICosmosDbQueryHandler, CosmosDbQueryHandler>();
    }

    private static void AddCosmosDbDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services),
                "A service collection is required to configure the CosmosDb Repository.");
        }

        services.ConfigureOptions<AzureCosmosDbOptions>(configuration);

        services.TryAddSingleton<ICosmosDbClientProvider, CosmosDbClientProvider>();
        services.TryAddSingleton(typeof(ICosmosDbContainerProvider), typeof(CosmosDbContainerProvider));
        services.TryAddSingleton(typeof(ICosmosDbQueryHandler<>), typeof(CosmosDbQueryHandler<>));
    }
}
