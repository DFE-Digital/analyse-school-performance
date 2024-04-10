using ASP.Core.Results;
using Azure;
using Azure.Data.Tables;

namespace ASP.Infrastructure.TableStorage
{
    public interface ITableStorageProvider
    {
        Task<TableClient> GetTable(string tableName);

        Task<Response> UpdateTable(TableClient tableClient,
            TableStorageEntry tableStorageEntry);
    }
}
