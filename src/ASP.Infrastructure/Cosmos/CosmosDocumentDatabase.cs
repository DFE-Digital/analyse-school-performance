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
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Failure("CosmosDocumentDatabase.UpsertAsync", ex.Message);
            }
        }
    }
}
