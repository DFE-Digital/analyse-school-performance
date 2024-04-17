using ASP.Core.Logging;
using ASP.Core.Results;
using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.TableStorage
{
    public class TableStorageProvider : ITableStorageProvider
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly TableStorageConfiguration _tableStorageConfiguration;

        public TableStorageProvider(IOptions<TableStorageConfiguration> tableStorageConfiguration)
        {
            _tableStorageConfiguration = tableStorageConfiguration.Value;
            _tableServiceClient = new TableServiceClient(_tableStorageConfiguration.ConnectionString);
        }

        public async Task<Result<string>> AddTableEntry(TableStorageEntry tableStorageEntry)
        {
            try
            {
                var tableClient = await CreateTable();

                await tableClient.AddEntityAsync(tableStorageEntry);

                return Result.Success("Error details added to " + tableClient.Name + " table with error code " + tableStorageEntry.RowKey);

            }
            catch (RequestFailedException exception)
            {
                return Error.Unexpected(exception.Message);
            }
        }

        private async Task<TableClient> CreateTable()
        {
            bool exists = false;
            await foreach (var table in _tableServiceClient.QueryAsync(t => t.Name == _tableStorageConfiguration.TableName))
            {
                exists = true;
            }

            if (!exists)
            {
               await _tableServiceClient.CreateTableAsync(_tableStorageConfiguration.TableName);

               return _tableServiceClient.GetTableClient(_tableStorageConfiguration.TableName);
            }

            return _tableServiceClient.GetTableClient(_tableStorageConfiguration.TableName);
        }
    }
}
