using ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;
using ASP.Infrastructure.Azure.CosmosDbSeeder.Helper;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly string _connectionString;
    private readonly ILogger<BlobStorageService> _logger;
    private const string BLOBS_FOLDER_NAME = "blobs";

    public BlobStorageService(string connectionString, ILogger<BlobStorageService> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task UploadAllToBlobStorageAsync()
    {
        try
        {
            _logger.LogInformation("Starting upload of all configs to blob storage");

            var blobsRootPath = Path.Combine(DirectoryHelper.GetSolutionDirectoryPath(), BLOBS_FOLDER_NAME);
            
            if (!Directory.Exists(blobsRootPath))
            {
                throw new DirectoryNotFoundException($"Blobs root directory not found at: {blobsRootPath}");
            }

            var containerDirectories = Directory.GetDirectories(blobsRootPath);
            
            if (!containerDirectories.Any())
            {
                _logger.LogWarning("No container directories found in {BlobsPath}", blobsRootPath);
                return;
            }

            var blobServiceClient = new BlobServiceClient(_connectionString);

            foreach (var containerPath in containerDirectories)
            {
                var containerName = Path.GetFileName(containerPath);
                var files = Directory.GetFiles(containerPath);

                if (!files.Any())
                {
                    _logger.LogInformation("No files found in container directory {ContainerName}", containerName);
                    continue;
                }

                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                foreach (var filePath in files)
                {
                    var fileName = Path.GetFileName(filePath);
                    var blobClient = containerClient.GetBlobClient(fileName);

                    _logger.LogInformation("Uploading {FileName} to {ContainerName} container", fileName, containerName);

                    await using var fileStream = File.OpenRead(filePath);
                    await blobClient.UploadAsync(fileStream, true);

                    _logger.LogInformation("Successfully uploaded {FileName} to {ContainerName} container", fileName, containerName);
                }
            }

            _logger.LogInformation("Completed uploading all configs to blob storage");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload configs to blob storage");
            throw;
        }
    }
}