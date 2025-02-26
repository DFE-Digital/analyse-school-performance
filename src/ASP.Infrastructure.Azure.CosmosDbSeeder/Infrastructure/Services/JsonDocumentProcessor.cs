using System.Text.Json;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Extensions;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Services;

public class JsonDocumentProcessor : IJsonDocumentProcessor
{
    private readonly ILogger<JsonDocumentProcessor> _logger;
    private readonly IJsonProcessor _jsonProcessor;

    public JsonDocumentProcessor(
        ILogger<JsonDocumentProcessor> logger,
        IJsonProcessor jsonProcessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonProcessor = jsonProcessor ?? throw new ArgumentNullException(nameof(jsonProcessor));
    }

    public async Task<DocumentProcessingStats> ProcessDirectoryFiles(
        string directoryPath,
        Container container,
        ContainerConfig config)
    {
        var stats = DocumentProcessingStats.Empty;
        var jsonFiles = Directory.GetFiles(directoryPath, "*.json");

        _logger.LogInformation("Found {FileCount} JSON files in directory {DirectoryPath}",
            jsonFiles.Length, directoryPath);

        foreach (var file in jsonFiles)
        {
            await ProcessSingleFile(file, container, config);
        }

        return stats;
    }

    public async Task<DocumentProcessingStats> ProcessSingleFile(
        string filePath,
        Container container,
        ContainerConfig config)
    {
        var stats = DocumentProcessingStats.Empty;

        try
        {
            var jsonContent = await File.ReadAllTextAsync(filePath);
            using var jsonDocument = JsonDocument.Parse(jsonContent);
            var root = jsonDocument.RootElement;

            if (root.ValueKind != JsonValueKind.Array)
            {
                _logger.LogError("Container file {FilePath} must contain a JSON array", filePath);
                return stats.AddSkipped();
            }

            return await ProcessJsonArray(root, container, config, filePath, stats);
        }
        catch (Exception ex) when (ex is JsonException || ex is IOException)
        {
            _logger.LogError(ex, "Error processing container file {FilePath}", filePath);
            return stats.AddSkipped();
        }
    }

    private async Task<DocumentProcessingStats> ProcessJsonArray(
        JsonElement root,
        Container container,
        ContainerConfig containerConfig,
        string filePath,
        DocumentProcessingStats stats)
    {
        var currentStats = stats;
        var documentCount = root.GetArrayLength();

        _logger.LogInformation(
            "Processing {DocumentCount} documents from container file {FilePath}",
            documentCount,
            filePath);

        foreach (var element in root.EnumerateArray())
        {
            currentStats = await ProcessJsonElement(
                element,
                container,
                containerConfig,
                filePath,
                currentStats);
        }

        _logger.LogInformation(
            "Completed processing container file {FilePath}. Stats: {Stats}",
            filePath,
            currentStats);

        return currentStats;
    }

    private async Task<DocumentProcessingStats> ProcessJsonElement(
        JsonElement element,
        Container container,
        ContainerConfig containerConfig,
        string filePath,
        DocumentProcessingStats stats)
    {
        try
        {
            var processedDocument = element.ToProcessedDictionary(_jsonProcessor);
            return await CreateCosmosDocument(
                processedDocument,
                container,
                containerConfig,
                stats);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error processing document in container file {FilePath}", filePath);
            return stats.AddSkipped();
        }
    }

    private async Task<DocumentProcessingStats> CreateCosmosDocument(
        Dictionary<string, object?> document,
        Container container,
        ContainerConfig containerConfig,
        DocumentProcessingStats stats)
    {
        try
        {
            EnsureDocumentId(document);
            string documentId = document["id"]?.ToString()!;

            string? partitionKeyValue = _jsonProcessor.ExtractPartitionKeyValue(
                document,
                containerConfig.PartitionKeyPath);

            if (string.IsNullOrEmpty(partitionKeyValue))
            {
                _logger.LogWarning(
                    "Document with ID {DocumentId} is missing the partition key value for path {PartitionKeyPath}",
                    documentId,
                    containerConfig.PartitionKeyPath);
                return stats.AddSkipped();
            }

            LogDocumentDetails(document, documentId, partitionKeyValue);

            await container.CreateItemAsync(
                document,
                new PartitionKey(partitionKeyValue));

            _logger.LogInformation(
                "Successfully created document with ID {DocumentId}",
                documentId);

            return stats.AddCreated();
        }
        catch (CosmosException ex)
        {
            _logger.LogError(ex,
                "Error creating document with ID {DocumentId}: {Message}",
                document["id"],
                ex.Message);
            return stats.AddSkipped();
        }
    }

    private void EnsureDocumentId(Dictionary<string, object?> document)
    {
        if (!document.ContainsKey("id") || string.IsNullOrWhiteSpace(document["id"]?.ToString()))
        {
            document["id"] = Guid.NewGuid().ToString();
            _logger.LogWarning("Generated new id for document: {DocumentId}", document["id"]);
        }
    }

    private void LogDocumentDetails(
        Dictionary<string, object?> document,
        string documentId,
        string partitionKeyValue)
    {
        _logger.LogDebug(
            "Processing document - ID: {DocumentId}, Partition Key: {PartitionKeyValue}",
            documentId,
            partitionKeyValue);

        _logger.LogDebug("Document content: {Document}",
            JsonSerializer.Serialize(document, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
    }
}