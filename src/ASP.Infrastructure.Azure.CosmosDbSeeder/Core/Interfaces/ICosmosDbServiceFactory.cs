namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;

public interface ICosmosDbServiceFactory
{
    ICosmosDbService CreateSourceService();
    ICosmosDbService CreateTargetService();
}