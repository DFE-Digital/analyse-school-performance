using ASP.Core.Utilities;

namespace ASP.Infrastructure.Cosmos
{
    public interface ICosmosDbQueryHandler
    {
        Task<TItem> ReadItemByIdAsync<TItem>(string containerKey, string id, string partitionKeyValue, CancellationToken cancellationToken = default)
             where TItem : class;

        Task<IEnumerable<TItem>> ReadIterableItemsAsync<TItem>(string containerKey, Func<IQueryable<TItem>, IQueryable<TItem>> query,
            CancellationToken cancellationToken = default);
        
        Task<PagedEnumerable<TItem>> ReadPagedIterableItemsAsync<TItem>(string containerKey, Func<IQueryable<TItem>, IQueryable<TItem>> query,
            int skip, int take, CancellationToken cancellationToken = default);
    }
}
