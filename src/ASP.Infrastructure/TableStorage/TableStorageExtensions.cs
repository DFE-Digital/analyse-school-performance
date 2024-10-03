using ASP.Core;
using ASP.Infrastructure.Azure.TableStorage;
using ASP.Infrastructure.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Infrastructure.TableStorage;

public static class TableStorageExtensions
{
    public static IServiceCollection ConfigureTableStorage(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.BindConfig<TableStorageOptions>(out var config);

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
        services.ConfigureOptions<TableStorageOptions>(configuration);

        services.RemoveAll<ITableStorageProvider>();
        services.TryAddSingleton<ITableStorageProvider, InMemoryTableStorageProvider>();
    }

    private static void ConfigureAzure(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<AzureTableStorageOptions>(configuration);

        services.RemoveAll<ITableStorageProvider>();
        services.TryAddSingleton<ITableStorageProvider, AzureTableStorageProvider>();
    }
}
