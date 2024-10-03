using ASP.Core.Results;

namespace ASP.Core
{
    public interface ITableStorageProvider
    {
        Task<Result<Done>> AddTableEntry(TableStorageEntry tableStorageEntry);
        Task<Result<bool>> Exists(string partitionKey);

        Task<Result<Done>> Clear();
    }
}
