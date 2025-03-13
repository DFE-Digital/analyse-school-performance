using ASP.Core;
using ASP.Infrastructure.DocumentDatabase;
using Newtonsoft.Json;

namespace ASP.Infrastructure.Azure.CosmosDb;

public sealed class AzureCosmosDbOptions : DocumentDatabaseOptions
{
    public string ManagedIdentityClientId { get; set; } = "";
    public string EndpointUri { get; set; } = "";
    public string PrimaryKey { get; set; } = "";
    public string DatabaseId { get; set; } = "";

    [JsonProperty("Containers")]
    public Dictionary<string, ContainerOptions> Containers { get; set; } = new();

    public ContainerOptions GetContainerOptions(string containerKey)
    {
        _ = Containers
            .TryGetValue(containerKey, out var container);

        if (container == null)
        {
            throw new InvalidOperationException(
                $"Container dictionary options with container key: {containerKey} not configured in options.");
        }

        return container;
    }
}