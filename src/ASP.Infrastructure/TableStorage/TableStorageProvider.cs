using ASP.Core.Results;
using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;

//using Azure.Data.Tables.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

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

                var response = await _tableClient.AddEntityAsync(tableStorageEntry);

               // string location;
               // var locationa = response. //.TryGetHeader("location", out location);

                return Result.Success(_tableStorageConfiguration.TableName + " updated with error code " + tableStorageEntry.RowKey);

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
