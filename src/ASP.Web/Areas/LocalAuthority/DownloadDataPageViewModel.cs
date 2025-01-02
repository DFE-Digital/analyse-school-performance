using ASP.Web.Features.DataDownloads;
using ASP.Web.Shared;

namespace ASP.Web.Areas.LocalAuthority;

public class DownloadDataPageViewModel
{
    public PageViewModel Page { get; }
    public DownloadDataViewModel DownloadData { get; }

    public DownloadDataPageViewModel(
        PageViewModel page,
        DownloadDataViewModel downloadData
    )
    {
        Page = page;
        DownloadData = downloadData;
    }
}
