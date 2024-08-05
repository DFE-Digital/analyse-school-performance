using ASP.Core.Results;
using Azure.Data.Tables;
using Azure.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.TableStorage
{
    public class TableStorageProvider : ITableStorageProvider
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly TableStorageConfiguration _tableStorageConfiguration;
        private readonly IHostEnvironment _hostEnvironment;

        public TableStorageProvider(IOptions<TableStorageConfiguration> tableStorageConfiguration, IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            
            _tableStorageConfiguration = tableStorageConfiguration.Value;

            // For the local development environment, we want to use the connection string to ensure that the _tableServiceClient functions correctly.
            if (_hostEnvironment.IsDevelopment())
            {
                _tableServiceClient = new TableServiceClient(_tableStorageConfiguration.ConnectionString);
            }
            else
            {
                var credentialOptions = new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = _tableStorageConfiguration.ManagedIdentityClientId
                };
                var credential = new DefaultAzureCredential(credentialOptions);
                _tableServiceClient = new TableServiceClient(
                    new Uri($"https://{_tableStorageConfiguration.StorageAccountName}.table.core.windows.net/"),
                    credential);
            }
        }

        public async Task<Result<string>> AddTableEntry(TableStorageEntry tableStorageEntry)
        {
            try
            {
                var tableClient = await CreateTable();

                await tableClient.AddEntityAsync(tableStorageEntry);

                return Result.Success("Error details added to " + tableClient.Name + " table with error code " + tableStorageEntry.RowKey);

            }
            catch (Exception exception)
            {
                return Error.Unexpected(exception.Message, exception.StackTrace);
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
