using ASP.Core.Results;

namespace ASP.Core
{
    public interface IDocumentDatabase
    {
        Task<Result<TItem>> GetAsync<TItem>(string container, string id, string partitionKeyValue) where TItem : class;
        Task<Result<IEnumerable<TItem>>> QueryAsync<TItem>(string container, Func<IQueryable<TItem>, IQueryable<TItem>> query) where TItem : class;
        Task<Result<Done>> UpsertAsync<TItem>(string container, string id, string partitionKeyValue, TItem item) where TItem : class;
        Task<Result<Done>> DeleteAllAsync(string container);
    }
}
