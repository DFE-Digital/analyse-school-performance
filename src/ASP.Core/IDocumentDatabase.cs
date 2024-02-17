using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Core
{
    public interface IDocumentDatabase
    {
        Task<ErrorOr<TItem>> GetAsync<TItem>(string container, string id, string partitionKeyValue) where TItem : class;
        Task<ErrorOr<IEnumerable<TItem>>> QueryAsync<TItem>(string container, Func<IQueryable<TItem>, IQueryable<TItem>> query) where TItem : class;
        Task<ErrorOr<Updated>> UpsertAsync<TItem>(string container, string id, string partitionKeyValue, TItem item) where TItem : class;
        Task<ErrorOr<Deleted>> DeleteAllAsync(string container);
    }
}
