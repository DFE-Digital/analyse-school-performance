using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Extensions;

public static class CosmosExtensions
{
    public static async Task<IEnumerable<T>> ExecuteQueryAsync<T>(
        this Container container,
        QueryDefinition query,
        ILogger logger,
        int maxItemCount = 100)
    {
        try
        {
            var results = new List<T>();
            var queryOptions = new QueryRequestOptions { MaxItemCount = maxItemCount };

            using var iterator = container.GetItemQueryIterator<T>(
                query,
                requestOptions: queryOptions);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
                logger.LogDebug("Retrieved {Count} documents from container", response.Count);
            }

            return results;
        }
        catch (CosmosException ex)
        {
            logger.LogError(ex,
                "Cosmos DB error executing query. StatusCode: {StatusCode}",
                ex.StatusCode);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error executing Cosmos DB query");
            throw;
        }
    }
}