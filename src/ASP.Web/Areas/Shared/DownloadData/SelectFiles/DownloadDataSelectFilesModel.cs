using ASP.Application.UseCases.Downloads.DTO;

namespace ASP.Web.Areas.Shared.DownloadData.SelectFiles;

public class DownloadDataSelectFilesModel
{
    public List<DownloadDto> AvailableDownloads { get; }

    public DownloadDataSelectFilesModel(
        List<DownloadDto> availableDownloads
    )
    {
        AvailableDownloads = availableDownloads;
    }
}