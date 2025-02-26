using System.Text.Json;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Extensions;

public static class JsonDocumentExtensions
{
    public static Dictionary<string, object?> ToProcessedDictionary(
        this JsonElement element,
        IJsonProcessor processor)
    {
        var documentObject = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            element.GetRawText(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (documentObject == null)
        {
            throw new JsonException("Failed to deserialize JSON element to dictionary");
        }

        var processedDocument = new Dictionary<string, object?>();
        foreach (var kvp in documentObject)
        {
            processedDocument[kvp.Key] = processor.ProcessJsonValue(kvp.Value);
        }

        return processedDocument;
    }
}