namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Models;

public class DatabaseConfig
{
    public List<ContainerConfig> Containers { get; set; } = null!;
}