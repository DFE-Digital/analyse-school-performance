using ASP.Core;
using ASP.Core.Helpers;
using ASP.Core.Results;
using ASP.Core.Utilities;

namespace ASP.Infrastructure.InMemory
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
            var items = _memoryStore.Get(container, new(id, partitionKeyValue))
                .MapError(e => e is NotFoundError
                    ? Error.NotFound($@"Could not find the object with id ""{id}"" and partition key ""{partitionKeyValue}"" in container ""{container}"".")
                    : e)
                .Then(item => JsonHelper.DeserializeNotNull<TItem>(item.Contents, ignoreMissingMembers: true));

            return Task.FromResult(items);
        }

        public Task<Result<IEnumerable<TItem>>> QueryAsync<TItem>(
            string container,
            Func<IQueryable<TItem>, IQueryable<TItem>> query,
            CancellationToken cancellationToken = default
        ) where TItem : class
        {
            var items = _memoryStore.GetAll(container)
                .Then(items => items
                    .Select(item => JsonHelper.DeserializeNotNull<TItem>(item.Contents, ignoreMissingMembers: true))
                    .Combine());

            var result = items.Map(all =>
            {
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
            var result = _memoryStore.GetAll(container)
                .Then(items => items
                    .Select(item => JsonHelper.DeserializeNotNull<TItem>(item.Contents, ignoreMissingMembers: true))
                    .Combine())
                .Map(all =>
                {
                    var items = query(all.AsQueryable()).ToList();
                    var (skip, take, validPage) = PageHelper.ConstructPagingRequest(items.Count, page, itemsPerPage);
                    return new ResultsPage<TItem>(validPage, itemsPerPage, items.Count, items.Skip(skip).Take(take));
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
