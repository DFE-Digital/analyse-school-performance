using ASP.Core.Results;
using ASP.Web.Core.Environment;
using Azure.Data.Tables;
using Azure.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.TableStorage
{
    public class TableStorageProvider : ITableStorageProvider
    {
        private readonly TableServiceClient _client;
        private readonly TableStorageOptions _options;
        private readonly IHostEnvironment _hostEnvironment;

        public TableStorageProvider(IOptions<TableStorageOptions> options, IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _options = options.Value;

            // We want to use Azure credentials for everything BUT local development.
            // For local development, we want to use the connection string for the _tableServiceClient.
            if (hostEnvironment.IsLocalDevelopment())
            {
                _client = new TableServiceClient($"DefaultEndpointsProtocol=https;AccountName={_options.StorageAccountName};AccountKey={_options.PrimaryKey};EndpointSuffix=core.windows.net");
            }
            else
            {
                var credentialOptions = new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = _options.ManagedIdentityClientId
                };
                var credential = new DefaultAzureCredential(credentialOptions);
                _client = new TableServiceClient(
                    new Uri($"https://{_options.StorageAccountName}.table.core.windows.net/"),
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
            await foreach (var table in _client.QueryAsync(t => t.Name == _options.TableName))
            {
                exists = true;
            }

            if (!exists)
            {
                await _client.CreateTableAsync(_options.TableName);

                return _client.GetTableClient(_options.TableName);
            }

            return _client.GetTableClient(_options.TableName);
        }
    }
}
