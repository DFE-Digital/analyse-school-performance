using System.Text.Json;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Services;

public class JsonProcessor : IJsonProcessor
{
    private readonly ILogger<JsonProcessor> _logger;

    public JsonProcessor(ILogger<JsonProcessor> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public object? ProcessJsonValue(JsonElement element)
    {
        try
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    return ProcessObject(element);

                case JsonValueKind.Array:
                    return ProcessArray(element);

                case JsonValueKind.String:
                    return ProcessString(element.GetString());

                case JsonValueKind.Number:
                    return ProcessNumber(element);

                case JsonValueKind.True:
                    return true;

                case JsonValueKind.False:
                    return false;

                case JsonValueKind.Null:
                    return null;

                default:
                    _logger.LogWarning("Unsupported JSON value kind: {ValueKind}", element.ValueKind);
                    return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing JSON value");
            throw;
        }
    }

    private Dictionary<string, object?> ProcessObject(JsonElement element)
    {
        var result = new Dictionary<string, object?>();
        foreach (var property in element.EnumerateObject())
        {
            var processedValue = ProcessJsonValue(property.Value);
            result[property.Name] = processedValue;
        }
        return result;
    }

    private List<object?> ProcessArray(JsonElement element)
    {
        return element.EnumerateArray()
            .Select(item => ProcessJsonValue(item))
            .ToList();
    }

    private string? ProcessString(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        // Add any string processing logic here if needed
        return value;
    }

    private object ProcessNumber(JsonElement element)
    {
        if (element.TryGetInt64(out long longValue))
            return longValue;
        return element.GetDouble();
    }

    public string? ExtractPartitionKeyValue(Dictionary<string, object?> documentObject, string partitionKeyPath)
    {
        if (string.IsNullOrEmpty(partitionKeyPath))
        {
            _logger.LogWarning("Partition key path is null or empty");
            return null;
        }

        var keyPath = partitionKeyPath.TrimStart('/');

        if (documentObject.TryGetValue(keyPath, out var partitionKeyValue))
        {
            var value = partitionKeyValue?.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                _logger.LogWarning("Partition key value for path {PartitionKeyPath} is null or empty",
                    partitionKeyPath);
                return null;
            }

            _logger.LogDebug(
                "Successfully extracted partition key value: {PartitionKeyValue} for path: {PartitionKeyPath}",
                value, partitionKeyPath);
            return value;
        }

        _logger.LogWarning("Partition key path {PartitionKeyPath} not found in document",
            partitionKeyPath);
        return null;
    }
}