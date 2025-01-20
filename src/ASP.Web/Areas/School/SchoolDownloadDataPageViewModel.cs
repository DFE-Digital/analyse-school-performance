using ASP.Web.Features.DataDownloads;

namespace ASP.Web.Areas.School;

public class SchoolDownloadDataPageViewModel
{
    public SchoolPageViewModel SchoolPage { get; }
    public DownloadDataViewModel DownloadData { get; }

    public SchoolDownloadDataPageViewModel(
        SchoolPageViewModel schoolPage,
        DownloadDataViewModel downloadData
    )
    {
        SchoolPage = schoolPage;
        DownloadData = downloadData;
    }
}
