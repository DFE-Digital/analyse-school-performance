using ASP.Core.Results;

namespace ASP.Core
{
    public interface IBlobStorage
    {
        Task<Result<List<string>>> ListAsync(string container, string basePath, CancellationToken cancellationToken = default);

        Task<Result<string>> DownloadAsStringAsync(string container, string path, CancellationToken cancellationToken = default);

        Task<Result<Stream>> DownloadAsStreamAsync(string container, string path, CancellationToken cancellationToken = default);

        Task<Result<Done>> UploadAsync(string container, string path, string fileContents, CancellationToken cancellationToken = default);

        Task<Result<Done>> Clear();
    }
}
