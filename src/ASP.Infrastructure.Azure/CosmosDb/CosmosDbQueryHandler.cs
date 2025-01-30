using ASP.Core.Pagination;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Providers;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace ASP.Infrastructure.Azure.CosmosDb
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
            CancellationToken cancellationToken = default
        ) where TItem : class
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
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            CancellationToken cancellationToken = default
        )
        {
            Container container =
                await _cosmosDbContainerProvider
                    .GetContainerAsync(containerKey).ConfigureAwait(false);

            var queryable = container.GetItemLinqQueryable<TItem>(false, null, new QueryRequestOptions { },
                new CosmosLinqSerializerOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase });
            var feedIterator = query(queryable).ToFeedIterator();

            return await ReadIterableItemsAsync(feedIterator, cancellationToken);
        }

        public async Task<ResultsPage<TItem>> ReadPagedItemsAsync<TItem>(
            string containerKey,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            int page,
            int itemsPerPage,
            CancellationToken cancellationToken = default
        )
        {
            Container container = await _cosmosDbContainerProvider
                .GetContainerAsync(containerKey).ConfigureAwait(false);

            var queryable = query(container.GetItemLinqQueryable<TItem>(false, null, new QueryRequestOptions { },
                new CosmosLinqSerializerOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase }));

            var totalCount = await queryable.CountAsync(cancellationToken);

            var (skip, take, validPage) = PageHelper.ConstructPagingRequest(totalCount, page, itemsPerPage);

            var feedIterator = queryable.Skip(skip).Take(take).ToFeedIterator();

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

            return new ResultsPage<TItem>(validPage, itemsPerPage, totalCount, items);
        }

        private static async Task<IEnumerable<TItem>> ReadIterableItemsAsync<TItem>(
            FeedIterator<TItem> feedIterator,
            CancellationToken cancellationToken = default
        )
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
