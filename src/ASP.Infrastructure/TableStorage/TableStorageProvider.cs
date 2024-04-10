using ASP.Core.Results;
using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.TableStorage
{
    public class TableStorageProvider : ITableStorageProvider
    {
        private readonly ILogger<TableStorageProvider> _logger;
        private readonly TableServiceClient _tableServiceClient;
        private readonly TableStorageConfiguration _tableStorageConfiguration;

        public TableStorageProvider(ILogger<TableStorageProvider> logger,
            IOptions<TableStorageConfiguration> tableStorageConfiguration)
        {
            _logger = logger;
            _tableStorageConfiguration = tableStorageConfiguration.Value;
            _tableServiceClient = new TableServiceClient(_tableStorageConfiguration.ConnectionString);
        }

        public async Task<TableClient> GetTable(string tableName)
        {
            TableClient tableClient = _tableServiceClient.GetTableClient(tableName);

            try
            {
                await tableClient.CreateIfNotExistsAsync();

                return tableClient;
            }
            catch (RequestFailedException exception)
            {
                _logger.LogError(exception.Message);
                throw;
            }
        }

        public async Task<Response> AddTableEntry(TableClient tableClient, 
            TableStorageEntry tableStorageEntry,
            CancellationToken cancellationToken)
        {
            return await tableClient.AddEntityAsync(tableStorageEntry, cancellationToken);
        }
    }
}
