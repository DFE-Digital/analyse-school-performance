using ASP.Core.Configuration;
using ASP.Infrastructure.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Infrastructure.DocumentDatabase;

public static class DocumentDatabaseExtensions
{
    private static readonly MemoryStore<DocumentDatabaseKey> _store = new ();

    public static IServiceCollection ConfigureDocumentDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<DocumentDatabaseOptions>(configuration);

        services.TryAdd(new ServiceDescriptor(typeof(MemoryStore<DocumentDatabaseKey>), _store));
        services.TryAddSingleton<IDocumentDatabase, InMemoryDocumentDatabase>();

        return services;
    }
}
