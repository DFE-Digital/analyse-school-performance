using ASP.Infrastructure.Azure.CosmosDbSeeder.Configuration;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Helper;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Extensions;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly CosmosClient _cosmosClient;
    private readonly string _databaseId;
    private readonly ILogger<CosmosDbService> _logger;
    private readonly IJsonDocumentProcessor _documentProcessor;
    private Database? _database;

    public CosmosDbService(
        IOptions<CosmosDbServiceOptions> options,
        ILogger<CosmosDbService> logger,
        IJsonDocumentProcessor documentProcessor)
    {
        if (options?.Value == null)
            throw new ArgumentNullException(nameof(options));

        var cosmosOptions = options.Value;
        cosmosOptions.Validate(nameof(CosmosDbServiceOptions));

        var clientOptions = new CosmosClientOptions
        {
            ConnectionMode = cosmosOptions.ConnectionMode,
            ConsistencyLevel = cosmosOptions.ConsistencyLevel,
            SerializerOptions = new CosmosSerializationOptions()
        };

        _cosmosClient = new CosmosClient(cosmosOptions.ConnectionString, clientOptions);
        _databaseId = cosmosOptions.DatabaseId;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _documentProcessor = documentProcessor ?? throw new ArgumentNullException(nameof(documentProcessor));

        _logger.LogInformation("Initialized CosmosDB service for database: {DatabaseId}", _databaseId);
    }

    public async Task InitializeDatabaseAsync(DatabaseConfig config)
    {
        _logger.LogInformation("Creating database '{DatabaseId}' if it doesn't exist...", _databaseId);
        var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseId);

        foreach (var containerConfig in config.Containers)
        {
            await InitializeContainerAsync(database, containerConfig);
        }
    }

    public async Task<Container> GetContainerAsync(string containerName)
    {
        try
        {
            _database ??= await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseId);
            return _database.GetContainer(containerName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting container: {ContainerName}", containerName);
            throw;
        }
    }

    public async Task<IEnumerable<Dictionary<string, object>>> GetDocumentsAsync(ContainerConfig containerConfig)
    {
        try
        {
            var container = await GetContainerAsync(containerConfig.Name);

            // Create query definition
            var query = CreateQueryDefinition(containerConfig);

            // Use the extension method to execute the query
            var documents = await container.ExecuteQueryAsync<Dictionary<string, object>>(
                query,
                _logger);

            _logger.LogInformation(
                "Retrieved {Count} documents from container {ContainerName}",
                documents.Count(),
                containerConfig.Name);

            return documents;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error retrieving documents from container: {ContainerName}",
                containerConfig.Name);
            throw;
        }
    }

    private QueryDefinition CreateQueryDefinition(ContainerConfig config)
    {
        var baseQuery = "SELECT * FROM c";
        var whereClause = !string.IsNullOrEmpty(config.WhereClause) ? $" WHERE {config.WhereClause}" : string.Empty;
        return new QueryDefinition($"{baseQuery}{whereClause}");
    }

    private async Task InitializeContainerAsync(Database database, ContainerConfig containerConfig)
    {
        _logger.LogInformation("Processing container '{ContainerName}'...", containerConfig.Name);

        var container = await CreateContainerIfNotExistsAsync(database, containerConfig);

        if (await IsContainerEmpty(container))
        {
            await LoadDataIntoContainerAsync(container, containerConfig);
        }
        else
        {
            _logger.LogInformation("Container '{ContainerName}' already contains data. Skipping...",
                containerConfig.Name);
        }
    }

    private async Task<Container> CreateContainerIfNotExistsAsync(Database database, ContainerConfig containerConfig)
    {
        var properties = new ContainerProperties
        {
            Id = containerConfig.Name,
            PartitionKeyPath = containerConfig.PartitionKeyPath
        };

        return await database.CreateContainerIfNotExistsAsync(
            properties,
            containerConfig.Throughput ?? 400);
    }

    private async Task<bool> IsContainerEmpty(Container container)
    {
        var query = container.GetItemQueryIterator<dynamic>("SELECT VALUE COUNT(1) FROM c");
        var count = (await query.ReadNextAsync()).First();
        return count == 0;
    }

    private async Task LoadDataIntoContainerAsync(Container container, ContainerConfig containerConfig)
    {
        try
        {
            var dataPath = DirectoryHelper.GetSolutionDataFolderPath(containerConfig.DataPath);

            if (!Directory.Exists(dataPath) && !File.Exists(dataPath))
            {
                throw new DirectoryNotFoundException($"The specified data path does not exist: {dataPath}");
            }

            var stats = Directory.Exists(dataPath)
                ? await _documentProcessor.ProcessDirectoryFiles(dataPath, container, containerConfig)
                : await _documentProcessor.ProcessSingleFile(dataPath, container, containerConfig);

            _logger.LogInformation(
                "Data loading completed. Documents created: {CreatedCount}, Documents skipped: {SkippedCount}",
                stats.Created, stats.Skipped);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading data into container {ContainerName}",
                containerConfig.Name);
            throw;
        }
    }
}