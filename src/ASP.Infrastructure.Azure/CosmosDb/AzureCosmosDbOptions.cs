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
    public List<Dictionary<string, ContainerOptions>> Containers { get; set; } = new();

    public ContainerOptions GetContainerOptions(string containerKey)
    {
        _ = TryGetContainerOptionsDictionary(containerKey)
            .TryGetValue(containerKey, out var container);

        if (container == null)
        {
            throw new InvalidOperationException(
                $"Container dictionary options with container key: {containerKey} not configured in options.");
        }

        return container;
    }

    private Dictionary<string, ContainerOptions> TryGetContainerOptionsDictionary(string containerKey) =>
        Containers
            .SingleOrDefault(containerOptionsDict =>
                containerOptionsDict.ContainsKey(containerKey)) ??
        throw new InvalidOperationException(
            $"Container with key: {containerKey} not configured in options.");
}