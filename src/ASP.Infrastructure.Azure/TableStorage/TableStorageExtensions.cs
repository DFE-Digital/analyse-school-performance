using ASP.Core.Configuration;
using ASP.Infrastructure.TableStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Infrastructure.Azure.TableStorage;

public static class TableStorageExtensions
{
    public static IServiceCollection ConfigureTableStorage(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.BindConfig<TableStorageOptions>(out var config);

        if (config.InMemory)
        {
            Infrastructure.TableStorage.TableStorageExtensions.ConfigureTableStorage(services, configuration);
        }
        else
        {
            ConfigureAzure(services, configuration);
        }

        return services;
    }

    private static void ConfigureAzure(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<AzureTableStorageOptions>(configuration);

        services.RemoveAll<ITableStorageProvider>();
        services.TryAddSingleton<ITableStorageProvider, AzureTableStorageProvider>();
    }
}
