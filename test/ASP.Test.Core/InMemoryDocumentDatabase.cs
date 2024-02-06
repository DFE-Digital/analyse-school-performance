using ASP.Core;
using ErrorOr;

namespace ASP.Test.Core
{
    public class InMemoryDocumentDatabase : IDocumentDatabase
    {
        private MemoryStore _memoryStore;

        public InMemoryDocumentDatabase(MemoryStore memoryStore)
        {
            _memoryStore = memoryStore;
        }

        public Task<ErrorOr<TItem>> GetAsync<TItem>(string container, string id, string partitionKeyValue) where TItem : class
        {
            var items = _memoryStore.Get<TItem>(container, id, partitionKeyValue);

            return Task.FromResult(items);
        }

        public Task<ErrorOr<IEnumerable<TItem>>> QueryAsync<TItem>(string container, Func<IQueryable<TItem>, IQueryable<TItem>> query) where TItem : class
        {
            var items = _memoryStore.GetAll<TItem>(container)
                .Then(all => query(all.AsQueryable()).AsEnumerable());

            return Task.FromResult(items);
        }

        public Task<ErrorOr<Updated>> UpsertAsync<TItem>(string container, string id, string partitionKeyValue, TItem item) where TItem : class
        {
            _memoryStore.Set(container, id, partitionKeyValue, item);

            return Task.FromResult(Result.Updated.ToErrorOr());
        }
    }
}
