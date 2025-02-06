using ASP.Api.Client.DataDownloads;

namespace ASP.Web.Features.DataDownloads;

public class DownloadDataSelectFilesViewModel : DownloadDataViewModel
{
    public List<Download> AvailableDownloads { get; }

    public DownloadDataSelectFilesViewModel(
        List<Download> availableDownloads
    )
    {
        AvailableDownloads = availableDownloads;
    }
}
