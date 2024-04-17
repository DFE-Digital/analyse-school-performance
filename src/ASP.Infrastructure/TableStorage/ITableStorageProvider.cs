using ASP.Core.Results;

namespace ASP.Infrastructure.TableStorage
{
    public interface ITableStorageProvider
    {
        Task<Result<string>> AddTableEntry(TableStorageEntry tableStorageEntry);
    }
}
