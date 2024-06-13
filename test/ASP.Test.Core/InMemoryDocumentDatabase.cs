using ASP.Core;
using ASP.Core.Results;

namespace ASP.Test.Core
{
    public class InMemoryDocumentDatabase : IDocumentDatabase
    {
        private MemoryStore _memoryStore;

        public InMemoryDocumentDatabase(MemoryStore memoryStore)
        {
            _memoryStore = memoryStore;
        }

        public Task<Result<TItem>> GetAsync<TItem>(string container, string id, string partitionKeyValue) where TItem : class
        {
            var items = _memoryStore.Get<TItem>(container, id, partitionKeyValue);

            return Task.FromResult(items);
        }

        public Task<Result<IEnumerable<TItem>>> QueryAsync<TItem>(string container, Func<IQueryable<TItem>, IQueryable<TItem>> query) where TItem : class
        {
            var items = _memoryStore.GetAll<TItem>(container);

            var result = items.Map(all => {
                return query(all.AsQueryable())
                    .AsEnumerable();
            });

            return Task.FromResult(result);
        }

        public Task<Result<Done>> UpsertAsync<TItem>(string container, string id, string partitionKeyValue, TItem item) where TItem : class
        {
            _memoryStore.Set(container, id, partitionKeyValue, item);

            return Task.FromResult(Result.Done.ToResult());
        }

        public Task<Result<Done>> DeleteAllAsync(string container)
        {
            _memoryStore.ClearContainer(container);

            return Task.FromResult(Result.Done.ToResult());
        }
    }
}
