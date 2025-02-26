using ASP.Infrastructure.Azure.CosmosDbSeeder.Models;
using Microsoft.Azure.Cosmos;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;

public interface ICosmosDbService
{
    Task InitializeDatabaseAsync(DatabaseConfig config);
    Task<Container> GetContainerAsync(string containerName);
    Task<IEnumerable<Dictionary<string, object>>> GetDocumentsAsync(
        ContainerConfig containerConfig);
}