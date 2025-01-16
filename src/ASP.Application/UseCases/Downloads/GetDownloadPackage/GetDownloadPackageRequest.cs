using ASP.Core.Utilities;

namespace ASP.Application.UseCases.Downloads.GetDownloadPackage
{
    public record GetDownloadPackageRequest(FileType FileType, List<string> DownloadIds);
}
