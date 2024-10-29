using ASP.Core.Results;
using ASP.Core.Time;
using ASP.Core.Utilities;
using System.IO.Compression;
using System.Text;

namespace ASP.Application.UseCases.Downloads.DownloadAsZip
{
    public class DownloadAsZipFile : IDownloadAsZipFile
    {
        private readonly CurrentTimeProvider _currentTimeProvider;

        public DownloadAsZipFile(CurrentTimeProvider currentTimeProvider)
        {
            _currentTimeProvider = currentTimeProvider;
        }

        public async Task<Result<FileStreamResponse>> HandleRequest(DownloadAsZipFileRequest request)
        {
            var zipStream = new MemoryStream();
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                await Task.WhenAll(request.DownloadIds.Select(id => AddFileToArchiveAsync(archive, id, request.FileType)));
            }

            zipStream.Position = 0;
            var zipFileResult = new FileStreamResponse($"{_currentTimeProvider.CurrentTime:yyyyMMdd_HHmmss}_asp_download.zip", zipStream, "application/zip");

            return zipFileResult;
        }

        private async Task AddFileToArchiveAsync(ZipArchive archive, string id, FileType fileType)
        {
            var fileName = $"{id}.{fileType.ToString().ToLowerInvariant()}";
            string fileContent = GenerateFileContent(id, fileType);
            var entry = archive.CreateEntry(fileName, CompressionLevel.Fastest);

            using var entryStream = entry.Open();
            using var streamWriter = new StreamWriter(entryStream);
            await streamWriter.WriteAsync(fileContent);
        }

        private string GenerateFileContent(string id, FileType fileType)
        {
            return fileType switch
            {
                FileType.CSV => GenerateTestCsvContent(id),
                FileType.TXT => GenerateTestCsvContent(id),
                FileType.XLS => GenerateTestCsvContent(id),
                _ => GenerateTestCsvContent(id) // Default to CSV
            };
        }

        private string GenerateTestCsvContent(string downloadId)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id,Name,Value");
            sb.AppendLine($"{downloadId},Test Name,123");
            sb.Append($"{downloadId},Another Name,456");

            return sb.ToString();
        }
    }
}