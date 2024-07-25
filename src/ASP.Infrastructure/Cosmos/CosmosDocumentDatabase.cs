using ASP.Core;
using ASP.Core.Results;
using ASP.Core.Utilities;
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
        public CosmosDocumentDatabase(ILogger<CosmosDocumentDatabase> logger,
            ICosmosDbContainerProvider containerProvider,
            ICosmosDbQueryHandler queryHandler)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));

            _containerProvider = containerProvider ??
                throw new ArgumentNullException(nameof(containerProvider));

            _queryHandler = queryHandler ??
                throw new ArgumentNullException(nameof(queryHandler));
        }

        public async Task<Result<TItem>> GetAsync<TItem>(string container, string id,
            string partitionKeyValue, CancellationToken cancellationToken = default) where TItem : class
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

        public async Task<Result<IEnumerable<TItem>>> QueryAsync<TItem>(string container, Func<IQueryable<TItem>, 
            IQueryable<TItem>> query, CancellationToken cancellationToken = default) where TItem : class
        {
            try
            {
                var result = await _queryHandler.ReadIterableItemsAsync<TItem>(container, query, cancellationToken);
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

        public async Task<Result<PagedEnumerable<TItem>>> QueryAsyncPaged<TItem>(string container,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            int skip, int take, CancellationToken cancellationToken = default) where TItem : class
        {
            try
            {
                var result = await _queryHandler.ReadPagedIterableItemsAsync<TItem>(container,
                    query, skip, take, cancellationToken);

                return Result.Success(new PagedEnumerable<TItem>(result.Items, result.TotalCount));
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

        public async Task<Result<Done>> UpsertAsync<TItem>(string container, string id, 
            string partitionKeyValue, TItem item, CancellationToken cancellationToken = default) where TItem : class
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

        public async Task<Result<Done>> DeleteAllAsync(string container, CancellationToken cancellationToken = default)
        {
            try
            {
                Container containerObject = await _containerProvider.GetContainerAsync(container);
                var properties = await containerObject.ReadContainerAsync();
                var partitionKeyPath = properties.Resource.PartitionKeyPath.Split('/').FirstOrDefault(p => p.Trim().Length > 0);

                var items = await _queryHandler.
                    ReadIterableItemsAsync<Dictionary<string, object>>(container, q => q, cancellationToken);

                foreach (var item in items)
                {
                    var id = (string)item["id"];
                    var partitionKey = (string)item[partitionKeyPath];

                    var response = await containerObject
                        .DeleteItemAsync<Dictionary<string, object>>(id, new PartitionKey(partitionKey), cancellationToken: cancellationToken);
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
                        return Error.Unexpected(ex.Message, ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Unexpected(ex.Message, ex.StackTrace);
            }
        }
    }
}
