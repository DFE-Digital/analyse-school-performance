using ASP.Core.Results;
using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace ASP.Infrastructure.TableStorage
{
    public class TableStorageProvider : ITableStorageProvider
    {
        private readonly ILogger<TableStorageProvider> _logger;
        private readonly TableClient _tableClient;
        private readonly TableStorageConfiguration _tableStorageConfiguration;

        public TableStorageProvider(ILogger<TableStorageProvider> logger,
            IOptions<TableStorageConfiguration> tableStorageConfiguration)
        {
            _logger = logger;
            _tableStorageConfiguration = tableStorageConfiguration.Value;
            _tableClient = new TableServiceClient(_tableStorageConfiguration.ConnectionString)
                .GetTableClient(_tableStorageConfiguration.TableName);
        }

        public async Task<Result<Response>> UpdateTable(TableStorageEntry tableStorageEntry)
        {
            try
            {
                var table = await CreateTable();

                var response = await _tableClient.AddEntityAsync(tableStorageEntry);

                return Result.Success(response);

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
