using ASP.Core.Results;
using ASP.Infrastructure.TableStorage;

namespace ASP.Web.AcceptanceTests.Services
{
    public class TestTableStorageProvider : ITableStorageProvider
    {
        public async Task<Result<string>> UpdateTable(TableStorageEntry tableStorageEntry)
        {
            return Result.Success("Message");
        }
    }
}
