using ASP.Application.UseCases.Downloads.DTO;

namespace ASP.Web.Features.DataDownloads;

public class DownloadDataSelectFilesViewModel : DownloadDataViewModel
{
    public List<DownloadDto> AvailableDownloads { get; }

    public DownloadDataSelectFilesViewModel(
        List<DownloadDto> availableDownloads
    )
    {
        AvailableDownloads = availableDownloads;
    }
}
