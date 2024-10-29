using ASP.Core;
using ASP.Core.Results;

namespace ASP.Application.UseCases.BlobStorageDemoFileDownload;

public class BlobStorageDemoFileDownload : IBlobStorageDemoFileDownload
{
    private readonly IBlobStorage _blobStorage;

    public BlobStorageDemoFileDownload(IBlobStorage blobStorage)
    {
        _blobStorage = blobStorage;
    }

    public Task<Result<FileStreamResponse>> HandleRequest(BlobStorageDemoFileDownloadRequest request)
    {
        var result =
            from stream in _blobStorage.DownloadStream(request.Container, request.Filepath)
            select new FileStreamResponse(Path.GetFileName(request.Filepath), stream, "text/csv");

        return Task.FromResult(result);
    }
}