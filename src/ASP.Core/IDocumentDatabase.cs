using ASP.Core.Results;
using ASP.Core.Utilities;

namespace ASP.Core
{
    public interface IDocumentDatabase
    {
        Task<Result<TItem>> GetAsync<TItem>(string container, string id, string partitionKeyValue,
            CancellationToken cancellationToken = default) where TItem : class;
        Task<Result<IEnumerable<TItem>>> QueryAsync<TItem>(string container, Func<IQueryable<TItem>,
            IQueryable<TItem>> query, CancellationToken cancellationToken = default) where TItem : class;
        Task<Result<PagedEnumerable<TItem>>> QueryAsyncPaged<TItem>(string container,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            int skip, int take, CancellationToken cancellationToken = default) where TItem : class;
        Task<Result<Done>> UpsertAsync<TItem>(string container, string id, string partitionKeyValue, TItem item,
            CancellationToken cancellationToken = default) where TItem : class;
        Task<Result<Done>> DeleteAllAsync(string container, CancellationToken cancellationToken = default);
    }
}
