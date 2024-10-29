using ASP.Core;
using ASP.Core.Results;
using ASP.Web.Core.Environment;
using Azure.Data.Tables;
using Azure.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.Azure.TableStorage
{
    public class AzureTableStorageProvider : ITableStorageProvider
    {
        private readonly AzureTableStorageOptions _options;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly TableServiceClient _client;

        public AzureTableStorageProvider(IOptions<AzureTableStorageOptions> options, IHostEnvironment hostEnvironment)
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

        public async Task<Result<Done>> AddTableEntry(TableStorageEntry tableStorageEntry)
        {
            try
            {
                var tableClient = await EnsureTableExists();

                await tableClient.AddEntityAsync(tableStorageEntry);

                return Result.Done;

            }
            catch (Exception exception)
            {
                return Error.Unexpected(exception.Message, exception.StackTrace);
            }
        }

        public async Task<Result<bool>> Exists(string partitionKey)
        {
            try
            {
                var tableClient = await EnsureTableExists();

                var results = tableClient.QueryAsync<TableStorageEntry>(e => e.PartitionKey == partitionKey, maxPerPage: 1);

                await foreach (var _ in results)
                {
                    return true;
                }

                return false;

            }
            catch (Exception exception)
            {
                return Error.Unexpected(exception.Message, exception.StackTrace);
            }
        }

        public Task<Result<Done>> Clear()
        {
            throw new NotImplementedException("Clear should not be implemented in a real blob store.");
        }

        private async Task<TableClient> EnsureTableExists()
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
