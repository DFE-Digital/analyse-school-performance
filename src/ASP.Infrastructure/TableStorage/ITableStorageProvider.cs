using ASP.Core.Results;
using Azure;

namespace ASP.Infrastructure.TableStorage
{
    public interface ITableStorageProvider
    {
        Task<Result<Response>> UpdateTable(TableStorageEntry tableStorageEntry);
    }
}
