using ASP.Core;
using ASP.Infrastructure.Azure.CosmosDb;
using ASP.Infrastructure.InMemory;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Handlers.Query;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CosmosDbClientProvider = ASP.Infrastructure.Azure.CosmosDb.CosmosDbClientProvider;
using CosmosDbContainerProvider = ASP.Infrastructure.Azure.CosmosDb.CosmosDbContainerProvider;

namespace ASP.Infrastructure.DocumentDatabase;

public static class DocumentDatabaseExtensions
{
    private static readonly MemoryStore<DocumentDatabaseKey> _store = new ();

    public static IServiceCollection ConfigureDocumentDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<DocumentDatabaseOptions>(configuration, out var config);

        if (config.InMemory)
        {
            ConfigureInMemory(services);
        }
        else
        {
            ConfigureCosmos(services, configuration);
        }

        return services;
    }

    private static void ConfigureInMemory(IServiceCollection services)
    {
        services.RemoveAll<IDocumentDatabase>();

        services.TryAdd(new ServiceDescriptor(typeof(MemoryStore<DocumentDatabaseKey>), _store));
        services.TryAddSingleton<IDocumentDatabase, InMemoryDocumentDatabase>();
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
