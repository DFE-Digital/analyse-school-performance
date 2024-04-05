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

        public async Task<TableClient> GetTable(string tableName = "ASPProdErrors")
        {
            TableClient tableClient = _tableServiceClient.GetTableClient(tableName);

            await tableClient.CreateIfNotExistsAsync();

            return tableClient;
        }
    }
}
