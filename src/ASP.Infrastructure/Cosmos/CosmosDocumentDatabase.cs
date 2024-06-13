using ASP.Core;
using ASP.Core.Results;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Providers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace ASP.Infrastructure.Cosmos
{
    public class CosmosDocumentDatabase : IDocumentDatabase
    {
        private readonly ILogger<CosmosDocumentDatabase> _logger;
        private readonly ICosmosDbContainerProvider _containerProvider;
        private readonly ICosmosDbQueryHandler _queryHandler;

        public CosmosDocumentDatabase(ILogger<CosmosDocumentDatabase> logger, ICosmosDbContainerProvider containerProvider, ICosmosDbQueryHandler queryHandler)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));

            _containerProvider = containerProvider ??
                throw new ArgumentNullException(nameof(containerProvider));

            _queryHandler = queryHandler ??
                throw new ArgumentNullException(nameof(queryHandler));
        }

        public async Task<Result<TItem>> GetAsync<TItem>(string containerKey, string id, string partitionKeyValue) where TItem : class
        {
            try
            {
                return await _queryHandler.ReadItemByIdAsync<TItem>(containerKey, id, partitionKeyValue);
            }
            catch (CosmosException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return Error.NotFound($@"Could not find the object with id ""{id}"" and partition key ""{partitionKeyValue}"" in container ""{containerKey}"".");

                    default:
                        _logger.LogCritical(ex.Message);
                        return Error.Unexpected(ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Unexpected(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<TItem>>> QueryAsync<TItem>(string containerKey, Func<IQueryable<TItem>, IQueryable<TItem>> query) where TItem : class
        {
            try
            {
                var result = await _queryHandler.ReadIterableItemsAsync(containerKey, query);

                result = result.ToList();

                return Result.Success(result);
            }
            catch (CosmosException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return Error.NotFound($@"Unable to find query result in container ""{containerKey}"".");

                    default:
                        _logger.LogCritical(ex.Message);
                        return Error.Unexpected(ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Unexpected(ex.Message);
            }
        }

        public async Task<Result<Done>> UpsertAsync<TItem>(string containerKey, string id, string partitionKeyValue, TItem item) where TItem : class
        {
            try
            {
                Container container = await _containerProvider.GetContainerAsync(containerKey);

                ItemResponse<TItem> response = await container
                    .UpsertItemAsync(item, new PartitionKey(partitionKeyValue));

                return Result.Done;
            }
            catch (CosmosException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return Error.NotFound($@"Could not find the object with id ""{id}"" and partition key ""{partitionKeyValue}"" in container ""{containerKey}"".");

                    default:
                        _logger.LogCritical(ex.Message);
                        return Error.Unexpected(ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Unexpected(ex.Message);
            }
        }

        public async Task<Result<Done>> DeleteAllAsync(string containerKey)
        {
            try
            {
                Container container = await _containerProvider.GetContainerAsync(containerKey);

                var items = await _queryHandler.ReadIterableItemsAsync<Dictionary<string, object>>(containerKey, q => q);

                foreach (var item in items)
                {
                    var id = (string)item["id"];

                    var response = await container
                        .DeleteItemAsync<Dictionary<string, object>>(id, new PartitionKey(id));

                    switch(response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.NotFound:
                            return Error.NotFound(response.ToString());

                        default:
                            return Error.Unexpected(response.ToString());
                    }
                }

                return Result.Done;
            }
            catch (CosmosException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return Error.NotFound(ex.Message);

                    default:
                        _logger.LogCritical(ex.Message);
                        return Error.Unexpected(ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Unexpected(ex.Message);
            }
        }
    }
}
