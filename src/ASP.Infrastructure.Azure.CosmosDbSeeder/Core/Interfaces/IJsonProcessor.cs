using System.Text.Json;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;

public interface IJsonProcessor
{
    object? ProcessJsonValue(JsonElement element);
    string? ExtractPartitionKeyValue(Dictionary<string, object?> document, string partitionKeyPath);
}