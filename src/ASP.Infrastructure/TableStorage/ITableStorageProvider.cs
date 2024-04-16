using ASP.Core.Results;
using Azure;

namespace ASP.Infrastructure.TableStorage
{
    public interface ITableStorageProvider
    {
        Task<Result<string>> AddTableEntry(TableStorageEntry tableStorageEntry);
    }
}
