using ASP.Core;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Providers;
using ErrorOr;
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

        public async Task<ErrorOr<TItem>> GetAsync<TItem>(string containerKey, string id, string partitionKeyValue) where TItem : class
        {
            try
            {
                return await _queryHandler.ReadItemByIdAsync<TItem>(containerKey, id, partitionKeyValue);
            }
            catch (CosmosException ex)
            {
                if(ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return Error.NotFound("CosmosDocumentDatabase.GetAsync", ex.Message);
                } else
                {
                    _logger.LogCritical(ex.Message);
                    return Error.Failure("CosmosDocumentDatabase.GetAsync", ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Failure("CosmosDocumentDatabase.GetAsync", ex.Message);
            }
        }

        public Task<ErrorOr<IEnumerable<TItem>>> QueryAsync<TItem>(string containerKey, Func<IQueryable<TItem>, IQueryable<TItem>> query) where TItem : class
        {
            throw new NotImplementedException();
        }

        public async Task<ErrorOr<Updated>> UpsertAsync<TItem>(string containerKey, string id, string partitionKeyValue, TItem item) where TItem : class
        {
            try
            {
                Container container = await _containerProvider.GetContainerAsync(containerKey);

                ItemResponse<TItem> response = await container
                    .UpsertItemAsync(item, new PartitionKey(partitionKeyValue));

                return Result.Updated;
            }
            catch (CosmosException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return Error.NotFound("CosmosDocumentDatabase.UpsertAsync", ex.Message);
                }
                else
                {
                    _logger.LogCritical(ex.Message);
                    return Error.Failure("CosmosDocumentDatabase.UpsertAsync", ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Failure("CosmosDocumentDatabase.UpsertAsync", ex.Message);
            }
        }

        public async Task<ErrorOr<Deleted>> DeleteAllAsync(string containerKey)
        {
            try
            {
                Container container = await _containerProvider.GetContainerAsync(containerKey);

                var items = await _queryHandler.ReadIterableItemsAsync<Dictionary<string, object>>(containerKey, q => q, q => true);

                foreach (var item in items)
                {
                    var id = (string)item["id"];

                    var response = await container
                        .DeleteItemAsync<Dictionary<string, object>>(id, new PartitionKey(id));

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return Error.Failure(response.StatusCode.ToString(), response.ToString());
                    }
                }

                return Result.Deleted;
            }
            catch (CosmosException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return Error.NotFound("CosmosDocumentDatabase.DeleteAllAsync", ex.Message);
                }
                else
                {
                    _logger.LogCritical(ex.Message);
                    return Error.Failure("CosmosDocumentDatabase.DeleteAllAsync", ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Failure("CosmosDocumentDatabase.DeleteAllAsync", ex.Message);
            }
        }
    }
}
