using ASP.Web.Features.DataDownloads;

namespace ASP.Web.Areas.School.ViewModels;

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
