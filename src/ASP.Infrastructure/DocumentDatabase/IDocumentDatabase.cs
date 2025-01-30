using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Infrastructure.DocumentDatabase
{
    public interface IDocumentDatabase
    {
        Task<Result<TItem>> GetAsync<TItem>(
            string container,
            string id,
            string partitionKeyValue,
            CancellationToken cancellationToken = default
        ) where TItem : class;

        Task<Result<IEnumerable<TItem>>> QueryAsync<TItem>(
            string container,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            CancellationToken cancellationToken = default
        ) where TItem : class;

        Task<Result<ResultsPage<TItem>>> QueryPagedAsync<TItem>(
            string container,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            int page,
            int itemsPerPage,
            CancellationToken cancellationToken = default
        ) where TItem : class;

        Task<Result<Done>> UpsertAsync<TItem>(
            string container,
            string id,
            string partitionKeyValue,
            TItem item,
            CancellationToken cancellationToken = default
        ) where TItem : class;

        Task<Result<Done>> Clear();
    }
}
