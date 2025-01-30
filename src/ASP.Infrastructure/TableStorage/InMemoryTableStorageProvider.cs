using ASP.Core.Results;

namespace ASP.Infrastructure.TableStorage
{
    public class InMemoryTableStorageProvider : ITableStorageProvider
    {
        private readonly List<TableStorageEntry> _tableStorageEntries = [];

        public Task<Result<Done>> AddTableEntry(TableStorageEntry tableStorageEntry)
        {
            _tableStorageEntries.Add(tableStorageEntry);

            return Task.FromResult(Result.Success(Result.Done));
        }

        public Task<Result<bool>> Exists(string partitionKey)
        {
            return Task.FromResult(Result.Success(_tableStorageEntries.Any(e => e.PartitionKey == partitionKey)));
        }

        public Task<Result<Done>> Clear()
        {
            _tableStorageEntries.Clear();

            return Task.FromResult(Result.Success(Result.Done));
        }
    }
}
