using ASP.Core.Utilities;

namespace ASP.Application.UseCases.Downloads.DownloadAsZip
{
    public record DownloadAsZipFileRequest(FileType FileType, List<string> DownloadIds);
}
