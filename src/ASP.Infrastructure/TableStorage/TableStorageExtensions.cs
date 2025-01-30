using ASP.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Infrastructure.TableStorage;

public static class TableStorageExtensions
{
    public static IServiceCollection ConfigureTableStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<TableStorageOptions>(configuration);

        services.RemoveAll<ITableStorageProvider>();
        services.TryAddSingleton<ITableStorageProvider, InMemoryTableStorageProvider>();

        return services;
    }
}
