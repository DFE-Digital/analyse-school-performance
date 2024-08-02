using ASP.Core;
using ASP.Core.Helpers;
using ASP.Core.Results;
using ASP.Core.Utilities;

namespace ASP.Test.Core
{
    public class InMemoryDocumentDatabase : IDocumentDatabase
    {
        private MemoryStore _memoryStore;

        public InMemoryDocumentDatabase(MemoryStore memoryStore)
        {
            _memoryStore = memoryStore;
        }

        public Task<Result<TItem>> GetAsync<TItem>(
            string container, 
            string id,
            string partitionKeyValue, 
            CancellationToken cancellationToken = default
        ) where TItem : class
        {
            var items = _memoryStore.Get<TItem>(container, id, partitionKeyValue);

            return Task.FromResult(items);
        }

        public Task<Result<IEnumerable<TItem>>> QueryAsync<TItem>(
            string container, 
            Func<IQueryable<TItem>, IQueryable<TItem>> query, 
            CancellationToken cancellationToken = default
        ) where TItem : class
        {
            var items = _memoryStore.GetAll<TItem>(container);

            var result = items.Map(all => {
                return query(all.AsQueryable())
                    .AsEnumerable();
            });

            return Task.FromResult(result);
        }

        public Task<Result<ResultsPage<TItem>>> QueryPagedAsync<TItem>(
            string container,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            int page, 
            int itemsPerPage, 
            CancellationToken cancellationToken = default
        ) where TItem : class
        {
            var (skip, take) = PageHelper.ConstructPagingRequest(page, itemsPerPage);

            var result = _memoryStore.GetAll<TItem>(container)
                .Map(all => {
                    var items = query(all.AsQueryable()).ToList();
                    return new ResultsPage<TItem>(page, itemsPerPage, items.Count, items.Skip(skip).Take(take));
                });
 
            return Task.FromResult(result);
        }

        public Task<Result<Done>> UpsertAsync<TItem>(
            string container, 
            string id, 
            string partitionKeyValue, 
            TItem item, 
            CancellationToken cancellationToken = default
        ) where TItem : class
        {
            _memoryStore.Set(container, id, partitionKeyValue, item);

            return Task.FromResult(Result.Done.ToResult());
        }

        public Task<Result<Done>> DeleteAllAsync(
            string container, 
            CancellationToken cancellationToken = default
        )
        {
            _memoryStore.ClearContainer(container);

            return Task.FromResult(Result.Done.ToResult());
        }
    }
}
