using ASP.Core;
using ASP.Core.Results;
using ASP.Web.Core.Environment;
using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ASP.Infrastructure.Azure.Blob
{
    public class AzureBlobStorage : IBlobStorage
    {
        private readonly AzureBlobStorageOptions _options;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly BlobServiceClient _client;

        public AzureBlobStorage(IOptions<AzureBlobStorageOptions> options, IHostEnvironment hostEnvironment)
        {
            _options = options.Value;
            _hostEnvironment = hostEnvironment;

            // We want to use Azure credentials for everything BUT local development.
            // For local development, we want to use the connection string for the _tableServiceClient.
            if (_hostEnvironment.IsLocalDevelopment())
            {
                _client = new BlobServiceClient($"DefaultEndpointsProtocol=https;AccountName={_options.StorageAccountName};AccountKey={_options.PrimaryKey};EndpointSuffix=core.windows.net");
            }
            else
            {
                var credentialOptions = new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = _options.ManagedIdentityClientId
                };
                var credential = new DefaultAzureCredential(credentialOptions);
                _client = new BlobServiceClient(
                    new Uri($"https://{_options.StorageAccountName}.blob.core.windows.net/"),
                    credential);
            }
        }


        public async Task<Result<List<string>>> ListAsync(string container, string basePath, CancellationToken cancellationToken = default)
        {
            try
            {
                var blobs = _client
                    .GetBlobContainerClient(container)
                    .GetBlobsAsync(prefix: basePath, cancellationToken: cancellationToken);

                var result = new List<string>();

                await foreach (BlobItem item in blobs)
                {
                    result.Add(item.Name);
                }

                if (!result.Any())
                {
                    return Error.NotFound($@"Could not find any blobs in container ""{container}"" starting with prefix ""{basePath}"".");
                }

                return result;
            }
            catch (RequestFailedException ex)
            {
                return Error.Unexpected("Azure request failed: " + ex.Message, ex.StackTrace);
            }
            catch (Exception ex)
            {
                return Error.Unexpected(ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result<Done>> DownloadToAsync(Stream stream, string container, string path, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _client
                    .GetBlobContainerClient(container)
                    .GetBlobClient(path)
                    .DownloadToAsync(stream, cancellationToken: cancellationToken);

                if (response.Status == 404)
                {
                    return Error.NotFound($@"Could not find a blob in container ""{container}"" with path ""{path}"".");
                }

                return Result.Done;
            }
            catch (RequestFailedException ex)
            {
                return Error.Unexpected("Azure request failed: " + ex.Message, ex.StackTrace);
            }
            catch (Exception ex)
            {
                return Error.Unexpected(ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result<BinaryData>> DownloadAsync(string container, string path, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _client
                    .GetBlobContainerClient(container)
                    .GetBlobClient(path)
                    .DownloadContentAsync(cancellationToken: cancellationToken);

                if (response.GetRawResponse().Status == 404)
                {
                    return Error.NotFound($@"Could not find a blob in container ""{container}"" with path ""{path}"".");
                }

                return response.Value.Content;
            }
            catch (RequestFailedException ex)
            {
                return Error.Unexpected("Azure request failed: " + ex.Message, ex.StackTrace);
            }
            catch (Exception ex)
            {
                return Error.Unexpected(ex.Message, ex.StackTrace);
            }
        }

        public Result<Stream> DownloadStream(string container, string path, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = _client
                    .GetBlobContainerClient(container)
                    .GetBlobClient(path)
                    .DownloadStreaming(new BlobDownloadOptions { }, cancellationToken: cancellationToken);

                if (response.GetRawResponse().Status == 404)
                {
                    return Error.NotFound($@"Could not find a blob in container ""{container}"" with path ""{path}"".");
                }

                return response.Value.Content;
            }
            catch (RequestFailedException ex)
            {
                return Error.Unexpected("Azure request failed: " + ex.Message, ex.StackTrace);
            }
            catch (Exception ex)
            {
                return Error.Unexpected(ex.Message, ex.StackTrace);
            }
        }

        public async Task<Result<Done>> UploadAsync(string container, string path, BinaryData fileContents, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _client
                    .GetBlobContainerClient(container)
                    .GetBlobClient(path)
                    .UploadAsync(fileContents, cancellationToken: cancellationToken);

                if (response.GetRawResponse().Status == 404)
                {
                    return Error.NotFound($@"Could not find a blob in container ""{container}"" with path ""{path}"".");
                }

                return Result.Done;
            }
            catch (RequestFailedException ex)
            {
                return Error.Unexpected("Azure request failed: " + ex.Message, ex.StackTrace);
            }
            catch (Exception ex)
            {
                return Error.Unexpected(ex.Message, ex.StackTrace);
            }
        }

        public Task<Result<Done>> ClearAsync()
        {
            throw new NotImplementedException("Clear should not be implemented in a real blob store.");
        }
    }
}
