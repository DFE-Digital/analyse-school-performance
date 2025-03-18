namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Configuration;

public class BlobStorageOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}