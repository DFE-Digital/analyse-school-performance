namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;

public interface IBlobStorageService
{
    Task UploadAllToBlobStorageAsync();
}