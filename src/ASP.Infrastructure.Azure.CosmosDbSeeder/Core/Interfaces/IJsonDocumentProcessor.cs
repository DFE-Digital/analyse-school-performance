using ASP.Infrastructure.Azure.CosmosDbSeeder.Models;
using Microsoft.Azure.Cosmos;

namespace ASP.Infrastructure.Azure.CosmosDbSeeder.Core.Interfaces;

public interface IJsonDocumentProcessor
{
    Task<DocumentProcessingStats> ProcessDirectoryFiles(string directoryPath, Container container, ContainerConfig config);
    Task<DocumentProcessingStats> ProcessSingleFile(string filePath, Container container, ContainerConfig config);
}