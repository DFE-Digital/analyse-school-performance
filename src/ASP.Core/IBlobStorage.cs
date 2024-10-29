using ASP.Core.Results;

namespace ASP.Core
{
    public interface IBlobStorage
    {
        Task<Result<List<string>>> ListAsync(string container, string basePath, CancellationToken cancellationToken = default);

        Task<Result<BinaryData>> DownloadAsync(string container, string path, CancellationToken cancellationToken = default);

        Task<Result<Done>> DownloadToAsync(Stream stream, string container, string path, CancellationToken cancellationToken = default);

        Result<Stream> DownloadStream(string container, string path, CancellationToken cancellationToken = default);

        Task<Result<Done>> UploadAsync(string container, string path, BinaryData fileContents, CancellationToken cancellationToken = default);

        Task<Result<Done>> Clear();
    }
}
