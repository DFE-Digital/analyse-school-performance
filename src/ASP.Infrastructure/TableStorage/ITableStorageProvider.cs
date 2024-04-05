using Azure.Data.Tables;

namespace ASP.Infrastructure.TableStorage
{
    public interface ITableStorageProvider
    {
        Task<TableClient> GetTable(string tableName);
    }
}
