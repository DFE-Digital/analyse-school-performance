using ASP.Core;
using ASP.Core.Results;
using ASP.Core.Time;
using System.IO.Compression;

namespace ASP.Application.UseCases.BlobStorageDemoZipFileDownload;

public class BlobStorageDemoZipFileDownload : IBlobStorageDemoZipFileDownload
{
    private readonly CurrentTimeProvider _currentTimeProvider;
    private readonly IBlobStorage _blobStorage;

    public BlobStorageDemoZipFileDownload(CurrentTimeProvider currentTimeProvider, IBlobStorage blobStorage)
    {
        _currentTimeProvider = currentTimeProvider;
        _blobStorage = blobStorage;
    }

    public async Task<Result<FileStreamResponse>> HandleRequest(BlobStorageDemoZipFileDownloadRequest request)
    {
        var zipStream = new MemoryStream();
        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            var entry = archive.CreateEntry(request.Filepath, CompressionLevel.Fastest);

            using var entryStream = entry.Open();
            await _blobStorage.DownloadToAsync(entryStream, request.Container, request.Filepath);
        }

        zipStream.Position = 0;

        var filename = $"{_currentTimeProvider.CurrentTime:yyyyMMdd_HHmmss}_asp_blob_storage_demo.zip";

        return new FileStreamResponse(filename, zipStream, "application/zip");
    }
}