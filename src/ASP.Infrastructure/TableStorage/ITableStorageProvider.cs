using ASP.Core.Results;

namespace ASP.Infrastructure.TableStorage
{
    public interface ITableStorageProvider
    {
        Task<Result<Done>> UpdateTable(string tableName, TableStorageEntry tableStorageEntry);
    }
}
