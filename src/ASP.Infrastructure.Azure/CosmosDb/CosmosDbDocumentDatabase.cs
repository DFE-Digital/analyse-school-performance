using ASP.Core;
using ASP.Core.Results;
using ASP.Core.Utilities;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Providers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace ASP.Infrastructure.Azure.CosmosDb
{
    public class CosmosDbDocumentDatabase : IDocumentDatabase
    {
        private readonly ILogger<CosmosDbDocumentDatabase> _logger;
        private readonly ICosmosDbContainerProvider _containerProvider;
        private readonly ICosmosDbQueryHandler _queryHandler;

        public CosmosDbDocumentDatabase(
            ILogger<CosmosDbDocumentDatabase> logger,
            ICosmosDbContainerProvider containerProvider,
            ICosmosDbQueryHandler queryHandler
        )
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));

            _containerProvider = containerProvider ??
                throw new ArgumentNullException(nameof(containerProvider));

            _queryHandler = queryHandler ??
                throw new ArgumentNullException(nameof(queryHandler));
        }

        public async Task<Result<TItem>> GetAsync<TItem>(
            string container,
            string id,
            string partitionKeyValue,
            CancellationToken cancellationToken = default
        ) where TItem : class
        {
            try
            {
                return await _queryHandler.ReadItemByIdAsync<TItem>(container, id, partitionKeyValue, cancellationToken);
            }
            catch (CosmosException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return Error.NotFound($@"Not found: could not find the object with id ""{id}"" and partition key ""{partitionKeyValue}"" in container ""{container}"".");

                    default:
                        _logger.LogCritical(ex.Message);
                        return Error.Unexpected(ex.Message, ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Unexpected(ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result<IEnumerable<TItem>>> QueryAsync<TItem>(
            string container,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            CancellationToken cancellationToken = default
        ) where TItem : class
        {
            try
            {
                var result = await _queryHandler.ReadIterableItemsAsync(container, query, cancellationToken);
                return Result.Success(result);
            }
            catch (CosmosException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return Error.NotFound($@"Not found: could not find the queried objects in container ""{container}"".");

                    default:
                        _logger.LogCritical(ex.Message);
                        return Error.Unexpected(ex.Message, ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Unexpected(ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result<ResultsPage<TItem>>> QueryPagedAsync<TItem>(
            string container,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            int page,
            int itemsPerPage,
            CancellationToken cancellationToken = default
        ) where TItem : class
        {
            try
            {
                var result = await _queryHandler.ReadPagedItemsAsync(container, query, page, itemsPerPage, cancellationToken);

                return Result.Success(result);
            }
            catch (CosmosException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return Error.NotFound(
                            $@"Not found: could not find the queried objects in container ""{container}"".");

                    default:
                        _logger.LogCritical(ex.Message);
                        return Error.Unexpected(ex.Message, ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Unexpected(ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result<Done>> UpsertAsync<TItem>(
            string container,
            string id,
            string partitionKeyValue,
            TItem item,
            CancellationToken cancellationToken = default
        ) where TItem : class
        {
            try
            {
                Container containerObject = await _containerProvider.GetContainerAsync(container);

                ItemResponse<TItem> response = await containerObject
                    .UpsertItemAsync(item, new PartitionKey(partitionKeyValue), cancellationToken: cancellationToken);

                return Result.Done;
            }
            catch (CosmosException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return Error.NotFound($@"Not found: could not find the object with id ""{id}"" and partition key ""{partitionKeyValue}"" in container ""{container}"".");

                    default:
                        _logger.LogCritical(ex.Message);
                        return Error.Unexpected(ex.Message, ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Unexpected(ex.Message, ex.StackTrace);
            }
        }

        public Task<Result<Done>> Clear()
        {
            throw new NotImplementedException("Clear should not be implemented in a real document database.");
        }
    }
}
