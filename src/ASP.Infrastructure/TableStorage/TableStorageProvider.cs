using ASP.Core.Results;
using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.TableStorage
{
    public class TableStorageProvider : ITableStorageProvider
    {
        private readonly TableClient _tableClient;
        private readonly TableStorageConfiguration _tableStorageConfiguration;

        public TableStorageProvider(IOptions<TableStorageConfiguration> tableStorageConfiguration)
        {
            _tableStorageConfiguration = tableStorageConfiguration.Value;
            _tableClient = new TableServiceClient(_tableStorageConfiguration.ConnectionString)
                .GetTableClient(_tableStorageConfiguration.TableName);
        }

        public async Task<Result<string>> UpdateTable(TableStorageEntry tableStorageEntry)
        {
            try
            {
                var table = await CreateTable();

                await _tableClient.AddEntityAsync(tableStorageEntry);

                return Result.Success(table.Value.Name + " updated with error code " + tableStorageEntry.RowKey);

            }
            catch (RequestFailedException exception)
            {
                return Error.Unexpected(exception.Message);
            }
        }

        private async Task<Response<TableItem>> CreateTable()
        {
            var response = await _tableClient.CreateIfNotExistsAsync();

            return response;
        }
    }
}
