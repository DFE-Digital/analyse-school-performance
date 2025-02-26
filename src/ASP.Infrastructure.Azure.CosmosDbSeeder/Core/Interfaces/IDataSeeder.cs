namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;

public interface IDataSeeder
{
    Task CleanupDataFolderAsync();
    Task ExtractAsync();
    Task SeedAsync();
    Task ExecuteAsync(); // This will run both extract and seed
}