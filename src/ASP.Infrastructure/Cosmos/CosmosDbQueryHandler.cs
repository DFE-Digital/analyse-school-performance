using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Providers;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;

namespace ASP.Infrastructure.Cosmos
{
    public sealed class CosmosDbQueryHandler : ICosmosDbQueryHandler
    {
        private readonly ICosmosDbContainerProvider _cosmosDbContainerProvider;

        public CosmosDbQueryHandler(ICosmosDbContainerProvider cosmosDbContainerProvider)
        {
            _cosmosDbContainerProvider = cosmosDbContainerProvider ??
                throw new ArgumentNullException(nameof(cosmosDbContainerProvider));
        }

        public async Task<TItem> ReadItemByIdAsync<TItem>(
            string containerKey,
            string id,
            string partitionKeyValue,
            CancellationToken cancellationToken = default)
               where TItem : class
        {
            Container container =
                await _cosmosDbContainerProvider
                    .GetContainerAsync(containerKey).ConfigureAwait(false);

            ItemResponse<TItem> response =
                await container.ReadItemAsync<TItem>(
                    id, new PartitionKey(partitionKeyValue),
                    cancellationToken: cancellationToken).ConfigureAwait(false);

            return response.Resource;
        }

        public async Task<IEnumerable<TItem>> ReadIterableItemsAsync<TItem>(
            string containerKey,
            Expression<Func<TItem, TItem>> selector,
            Expression<Func<TItem, bool>> predicate,
            CancellationToken cancellationToken = default)
               where TItem : class
        {
            Container container =
                await _cosmosDbContainerProvider
                    .GetContainerAsync(containerKey).ConfigureAwait(false);

            return await ReadIterableItemsAsync(
                container.GetItemLinqQueryable<TItem>()
                    .Where(predicate)
                    .Select(selector)
                        .ToFeedIterator(), cancellationToken);
        }

        public async Task<IEnumerable<TItem>> ReadIterableItemsAsync<TItem>(
            string containerKey,
            QueryDefinition queryDefinition,
            CancellationToken cancellationToken = default)
               where TItem : class
        {
            Container container =
                await _cosmosDbContainerProvider
                    .GetContainerAsync(containerKey).ConfigureAwait(false);

            return await ReadIterableItemsAsync(
                container.GetItemQueryIterator<TItem>(queryDefinition), cancellationToken);
        }

        private static async Task<IEnumerable<TItem>> ReadIterableItemsAsync<TItem>(
            FeedIterator<TItem> feedIterator,
            CancellationToken cancellationToken = default)
               where TItem : class
        {
            var items = new List<TItem>();

            using (feedIterator)
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<TItem> response =
                        await feedIterator
                            .ReadNextAsync(cancellationToken).ConfigureAwait(false);

                    items.AddRange(response.Resource);
                }
            }

            return items;
        }
    }
}
