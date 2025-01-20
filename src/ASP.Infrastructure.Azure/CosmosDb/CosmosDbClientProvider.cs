using ASP.Web.Core.Environment;
using Azure.Identity;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Providers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.Azure.CosmosDb;

public sealed class CosmosDbClientProvider : ICosmosDbClientProvider, IDisposable
{
    private readonly Lazy<CosmosClient> _lazyCosmosClient;
    private readonly AzureCosmosDbOptions _azureCosmosDbOptions;
    private readonly IHostEnvironment _hostEnvironment;

    public CosmosDbClientProvider(IOptions<AzureCosmosDbOptions> repositoryOptions, IHostEnvironment hostEnvironment)
    {
        if (repositoryOptions == null)
        {
            throw new ArgumentNullException(nameof(repositoryOptions));
        }
        
        _hostEnvironment = hostEnvironment;
        _azureCosmosDbOptions = repositoryOptions.Value;
        _lazyCosmosClient = new Lazy<CosmosClient>(CreateCosmosClientInstance);
    }

    public Task<TItem> InvokeCosmosClientAsync<TItem>(
        Func<CosmosClient, Task<TItem>> clientInvoker) =>
        clientInvoker.Invoke(_lazyCosmosClient.Value);

    public void Dispose()
    {
        if (_lazyCosmosClient.IsValueCreated){
            _lazyCosmosClient.Value?.Dispose();
        }
    }

    private CosmosClient CreateCosmosClientInstance()
    {
        // We want to use Azure credentials for everything BUT local development.
        // For local development, we want to use the PrimaryKey string for the CosmosClient.
        if (_hostEnvironment.IsLocalDevelopment())
        {
            return new CosmosClient(
                _azureCosmosDbOptions.EndpointUri,
                _azureCosmosDbOptions.PrimaryKey,
                new CosmosClientOptions() {
                    ConnectionMode = ConnectionMode.Gateway }); 
        }
        
        var credentialOptions = new DefaultAzureCredentialOptions
        {
            ManagedIdentityClientId = _azureCosmosDbOptions.ManagedIdentityClientId
        };
        var credential = new DefaultAzureCredential(credentialOptions);

        return new CosmosClient(_azureCosmosDbOptions.EndpointUri, credential, new CosmosClientOptions()
        {
            ConnectionMode = ConnectionMode.Gateway
        });

    }
       
}