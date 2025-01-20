namespace ASP.Infrastructure.Azure.CosmosDb;

public sealed class ContainerOptions
{
    public string ContainerName { get; set; } = "";
    public string PartitionKey { get; set; } = "";
}