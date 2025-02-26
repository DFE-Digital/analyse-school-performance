using Microsoft.Azure.Cosmos;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Configuration;

public class CosmosDbServiceOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseId { get; set; } = string.Empty;

    public void Validate(string optionsName)
    {
        if (string.IsNullOrEmpty(ConnectionString))
            throw new ArgumentException($"{optionsName}: ConnectionString is required");

        if (string.IsNullOrEmpty(DatabaseId))
            throw new ArgumentException($"{optionsName}: DatabaseId is required");
    }

    public ConnectionMode ConnectionMode { get; set; } = ConnectionMode.Gateway;
    public ConsistencyLevel ConsistencyLevel { get; set; } = ConsistencyLevel.Session;
}