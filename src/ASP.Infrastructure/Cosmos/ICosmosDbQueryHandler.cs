using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Handlers.Query;
using Microsoft.Azure.Cosmos;
using System.Linq.Expressions;

namespace ASP.Infrastructure.Cosmos
{
    public interface ICosmosDbQueryHandler
    {
        Task<TItem> ReadItemByIdAsync<TItem>(string containerKey, string id, string partitionKeyValue, CancellationToken cancellationToken = default)
             where TItem : class;

        Task<IEnumerable<TItem>> ReadIterableItemsAsync<TItem>(string containerKey, QueryDefinition queryDefinition, CancellationToken cancellationToken = default)
            where TItem : class;

        Task<IEnumerable<TItem>> ReadIterableItemsAsync<TItem>(string containerKey, Expression<Func<TItem, TItem>> selector, Expression<Func<TItem, bool>> predicate, CancellationToken cancellationToken = default)
            where TItem : class;
    }
}
