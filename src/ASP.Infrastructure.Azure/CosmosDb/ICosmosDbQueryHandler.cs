using ASP.Core.Utilities;

namespace ASP.Infrastructure.Azure.CosmosDb
{
    public interface ICosmosDbQueryHandler
    {
        Task<TItem> ReadItemByIdAsync<TItem>(
            string containerKey,
            string id,
            string partitionKeyValue,
            CancellationToken cancellationToken = default
        ) where TItem : class;

        Task<IEnumerable<TItem>> ReadIterableItemsAsync<TItem>(
            string containerKey,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            CancellationToken cancellationToken = default
        );

        Task<ResultsPage<TItem>> ReadPagedItemsAsync<TItem>(
            string containerKey,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            int page,
            int itemsPerPage,
            CancellationToken cancellationToken = default
        );
    }
}
