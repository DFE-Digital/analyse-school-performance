using ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Helper;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder;

public class DataSeeder : IDataSeeder
{
    private readonly ILogger<DataSeeder> _logger;
    private readonly ICosmosDbServiceFactory _cosmosDbServiceFactory;
    private readonly DatabaseConfig _databaseConfig;
    private readonly ExtractionConfig _extractionConfig;

    public DataSeeder(
        ILogger<DataSeeder> logger,
        ICosmosDbServiceFactory cosmosDbServiceFactory,
        DatabaseConfig databaseConfig,
        ExtractionConfig extractionConfig)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cosmosDbServiceFactory =
            cosmosDbServiceFactory ?? throw new ArgumentNullException(nameof(cosmosDbServiceFactory));
        _databaseConfig = databaseConfig ?? throw new ArgumentNullException(nameof(databaseConfig));
        _extractionConfig = extractionConfig ?? throw new ArgumentNullException(nameof(extractionConfig));
    }

    public async Task ExecuteAsync()
    {
        try
        {
            _logger.LogInformation("Starting data management process...");

            // Extract if enabled
            if (_extractionConfig.Enabled)
            {
                if (_extractionConfig.CleanupBeforeExtract)
                {
                    await CleanupDataFolderAsync();
                }

                await ExtractAsync();
            }

            // Proceed with seeding
            await SeedAsync();

            _logger.LogInformation("Data management process completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during data management process.");
            throw;
        }
    }

    public async Task ExtractAsync()
    {
        try
        {
            _logger.LogInformation("Starting data extraction process...");

            // Get source service for extraction
            var sourceService = _cosmosDbServiceFactory.CreateSourceService();

            foreach (var containerConfig in _databaseConfig.Containers)
            {
                await ExtractContainerDataAsync(sourceService, containerConfig);
            }

            _logger.LogInformation("Data extraction completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during data extraction.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Starting data seeding process...");

            // Get target service for seeding
            var targetService = _cosmosDbServiceFactory.CreateTargetService();
            await targetService.InitializeDatabaseAsync(_databaseConfig);

            _logger.LogInformation("Data seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during data seeding.");
            throw;
        }
    }

    private async Task ExtractContainerDataAsync(
        ICosmosDbService sourceService,
        ContainerConfig containerConfig)
    {
        try
        {
            _logger.LogInformation("Starting extraction for container: {ContainerName}",
                containerConfig.Name);

            var documents =
                await sourceService.GetDocumentsAsync(containerConfig);

            // Process and save documents
            await ProcessAndSaveDocuments(documents, containerConfig);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract data from container: {ContainerName}",
                containerConfig.Name);
            throw;
        }
    }

    public Task CleanupDataFolderAsync()
    {
        try
        {
            _logger.LogInformation("Starting cleanup of data folders...");

            foreach (var containerConfig in _databaseConfig.Containers)
            {
                var dataPath = DirectoryHelper.GetSolutionDataFolderPath(containerConfig.DataPath);

                if (Directory.Exists(dataPath))
                {
                    _logger.LogInformation("Cleaning up folder: {FolderPath}", dataPath);

                    var filePath = Path.Combine(dataPath, $"{containerConfig.Name}.json");
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            File.Delete(filePath);
                            _logger.LogDebug("Deleted file: {FileName}", Path.GetFileName(filePath));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(
                                ex,
                                "Failed to delete file: {FileName}. Continuing with cleanup...",
                                Path.GetFileName(filePath));
                        }
                    }
                }
                else
                {
                    _logger.LogInformation(
                        "Data folder does not exist, creating: {FolderPath}",
                        dataPath);
                    Directory.CreateDirectory(dataPath);
                }
            }

            _logger.LogInformation("Data folder cleanup completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during data folder cleanup.");
            throw;
        }

        return Task.CompletedTask;
    }

    private Dictionary<string, object> CleanDocument(
        Dictionary<string, object> document,
        IEnumerable<string> excludeProperties)
    {
        return document
            .Where(kvp => !excludeProperties.Contains(kvp.Key))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private async Task ProcessAndSaveDocuments(
        IEnumerable<Dictionary<string, object>> documents,
        ContainerConfig containerConfig)
    {
        var dataPath = DirectoryHelper.GetSolutionDataFolderPath(containerConfig.DataPath);

        Directory.CreateDirectory(dataPath);

        var filePath = Path.Combine(dataPath, $"{containerConfig.Name}.json");

        try
        {
            // Clean all documents
            var cleanedDocuments = documents.Select(doc =>
                    CleanDocument(doc, _extractionConfig.ExcludeProperties ?? new List<string>()))
                .ToList();

            // Serialize with Newtonsoft.Json
            var jsonString = JsonConvert.SerializeObject(
                cleanedDocuments,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            await File.WriteAllTextAsync(filePath, jsonString);

            _logger.LogInformation(
                "Saved {DocumentCount} documents to file: {FilePath}",
                cleanedDocuments.Count,
                filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error saving documents to file: {FilePath}",
                filePath);
            throw;
        }
    }
}