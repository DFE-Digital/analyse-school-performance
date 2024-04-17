using ASP.Core.Results;
using ASP.Infrastructure.TableStorage;

namespace ASP.Web.AcceptanceTests.Services
{
    public class TestTableStorageProvider : ITableStorageProvider
    {
        public readonly List<TableStorageEntry> _tableStorageEntry = [];

        public async Task<Result<string>> AddTableEntry(TableStorageEntry tableStorageEntry)
        {
            _tableStorageEntry.Add(tableStorageEntry);

            return Result.Success("Message");
        }

        public void ClearTableStorage()
        {
            _tableStorageEntry.Clear();
        }
    }
}
