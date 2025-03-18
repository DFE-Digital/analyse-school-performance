using System.Text.Json;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Helper;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Extensions;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Services;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder;

public class Program
{
    static async Task Main(string[] args)
    {
        var configBuilder = new ConfigurationBuilder();

        // Build new configuration
        IConfiguration configuration = configBuilder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.secrets.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        // Setup Dependency Injection
        var serviceProvider = ConfigureServices(configuration);

        // Resolve the logger
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            // Resolve the data seeder and run the seeding process
            var dataSeeder = serviceProvider.GetRequiredService<IDataSeeder>();
            await dataSeeder.ExecuteAsync();

            logger.LogInformation("Data seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during data seeding.");
        }
        finally
        {
            // Dispose the service provider to release resources
            if (serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

    private static ServiceProvider ConfigureServices(IConfiguration configuration)
    {
        // Setup DI container
        var services = new ServiceCollection();

        // Add configuration
        services.AddSingleton(configuration);

        // Add logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddFile("logs/seeder-{Date}.log");
        });

        // Add options for CosmosDbService
        services.AddCosmosDbServices(configuration);

        // Register services
        services.AddSingleton<IJsonProcessor, JsonProcessor>();
        
        services.AddSingleton<IDataSeeder, DataSeeder>();

        // Register DatabaseConfig as a singleton
        services.AddSingleton(sp =>
        {
            var configPath = DirectoryHelper.GetSolutionFolderPath("data/config.json");
            var configJson = File.ReadAllText(configPath);
            var config = JsonSerializer.Deserialize<DatabaseConfig>(configJson)
                ?? throw new InvalidOperationException("Failed to load database configuration");
            return config;
        });
        
        services.AddBlobStorageServices(configuration);

        return services.BuildServiceProvider();
    }
}