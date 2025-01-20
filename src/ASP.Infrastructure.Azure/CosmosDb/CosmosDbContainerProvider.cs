using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Providers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.Azure.CosmosDb;

public sealed class CosmosDbContainerProvider : ICosmosDbContainerProvider
    {
        private readonly ILogger<CosmosDbContainerProvider> _logger;
        private readonly ICosmosDbClientProvider _cosmosClientProvider;
        private readonly AzureCosmosDbOptions _azureCosmosDbOptions;

        public CosmosDbContainerProvider(
            ILogger<CosmosDbContainerProvider> logger,
            ICosmosDbClientProvider cosmosClientProvider,
            IOptions<AzureCosmosDbOptions> repositoryOptions)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));

            _cosmosClientProvider = cosmosClientProvider ??
                throw new ArgumentNullException(nameof(cosmosClientProvider));

            if (repositoryOptions == null)
            {
                throw new ArgumentNullException(nameof(repositoryOptions));
            }

            _azureCosmosDbOptions = repositoryOptions.Value;
        }

        public async Task<Container> GetContainerAsync(string containerKey)
        {
            try
            {
                ContainerOptions containerOptions =
                    _azureCosmosDbOptions.GetContainerOptions(containerKey);

                Database database =
                    await _cosmosClientProvider.InvokeCosmosClientAsync(
                        client =>
                            client.CreateDatabaseIfNotExistsAsync(
                                _azureCosmosDbOptions.DatabaseId)).ConfigureAwait(false);

                return (Container)await
                    database.CreateContainerIfNotExistsAsync(
                        containerOptions.ContainerName, containerOptions.PartitionKey).ConfigureAwait(false);
            }
            catch (CosmosException cosmosEx)
            {
                _logger.LogError(
                    cosmosEx, "A Cosmos db error has occurred retrieving the container specified.");

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, "An error has occurred retrieving the container specified.");

                throw;
            }
        }
    }
