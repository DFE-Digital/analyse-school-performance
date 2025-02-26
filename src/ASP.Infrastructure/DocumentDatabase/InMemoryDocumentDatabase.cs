using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Core.Text;
using ASP.Infrastructure.InMemory;

namespace ASP.Infrastructure.DocumentDatabase
{
    public class InMemoryDocumentDatabase : IDocumentDatabase
    {
        private MemoryStore<DocumentDatabaseKey> _memoryStore;

        public InMemoryDocumentDatabase(MemoryStore<DocumentDatabaseKey> memoryStore)
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
            var result =
                from item in _memoryStore.Get(container, new(id, partitionKeyValue))
                    .MapErrorIf(e => e is NotFoundError, Error.NotFound($@"Could not find the object with id ""{id}"" and partition key ""{partitionKeyValue}"" in container ""{container}""."))
                from deserialized in JsonHelper.DeserializeNotNull<TItem>(item.Contents, ignoreMissingMembers: true)
                select deserialized;

            return Task.FromResult(result);
        }

        public Task<Result<IEnumerable<TItem>>> QueryAsync<TItem>(
            string container,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            CancellationToken cancellationToken = default
        ) where TItem : class
        {
            var result =
                from items in _memoryStore.GetAll(container)
                from deserialized in items
                    .Select(item => JsonHelper.DeserializeNotNull<TItem>(item.Contents, ignoreMissingMembers: true))
                    .Combine()
                select query(deserialized.AsQueryable())
                    .AsEnumerable();

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
            var result =
                from items in _memoryStore.GetAll(container)
                from deserialized in items
                    .Select(item => JsonHelper.DeserializeNotNull<TItem>(item.Contents, ignoreMissingMembers: true))
                    .Combine()
                let all = query(deserialized.AsQueryable()).ToList()
                let paging = PageHelper.ConstructPagingRequest(all.Count, page, itemsPerPage)
                select new ResultsPage<TItem>(paging.ValidPage, itemsPerPage, all.Count, all.Skip(paging.Skip).Take(paging.Take));

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
            _memoryStore.Set(container, new(id, partitionKeyValue), item);

            return Task.FromResult(Result.Success(Result.Done));
        }

        public Task<Result<Done>> Clear()
        {
            _memoryStore.Clear();

            return Task.FromResult(Result.Success(Result.Done));
        }
    }
}
