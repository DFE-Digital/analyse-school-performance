using ASP.Core;
using ASP.Core.Results;

namespace ASP.Infrastructure.Azure.Blob
{
    public class AzureBlobStorage : IBlobStorage
    {
        public Task<Result<Stream>> DownloadAsStreamAsync(string container, string path, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Not implemented yet...");
        }

        public Task<Result<string>> DownloadAsStringAsync(string container, string path, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Not implemented yet...");
        }

        public Task<Result<List<string>>> ListAsync(string container, string basePath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Not implemented yet...");
        }

        public Task<Result<Done>> UploadAsync(string container, string path, string fileContents, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Not implemented yet...");
        }

        public Task<Result<Done>> Clear()
        {
            throw new NotImplementedException("Clear should not be implemented in a real blob store.");
        }
    }
}
