using ASP.Core;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Infrastructure.DocumentDatabase;

public static class DocumentDatabaseExtensions
{
    private static readonly MemoryStore _store = new ();

    public static IServiceCollection ConfigureDocumentDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<DocumentDatabaseOptions>(configuration, out var config);

        if (config.InMemory)
        {
            ConfigureInMemory(services);
        } else
        {
            ConfigureCosmos(services);
        }

        return services;
    }

    private static void ConfigureInMemory(IServiceCollection services)
    {
        services.RemoveAll<IDocumentDatabase>();
        services.TryAdd(new ServiceDescriptor(typeof(MemoryStore), _store)); ;
        services.TryAddScoped<IDocumentDatabase, InMemoryDocumentDatabase>();
    }

    private static void ConfigureCosmos(IServiceCollection services)
    {
        services.RemoveAll<IDocumentDatabase>();
        services.AddCosmosDbDependencies();

        services.AddOptions<RepositoryOptions>().Configure(delegate (RepositoryOptions settings, IConfiguration configuration)
        {
            configuration.GetSection("CosmosDb").Bind(settings);
        });
        services.TryAddScoped<IDocumentDatabase, CosmosDbDocumentDatabase>();
        services.TryAddScoped<ICosmosDbQueryHandler, CosmosDbQueryHandler>();
    }
}
