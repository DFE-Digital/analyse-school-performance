using ASP.Web.Areas.Shared.DownloadData.SelectYear;

namespace ASP.Web.Areas.School.ViewModels;

public class SchoolDownloadDataSelectYearViewModel
{
    public SchoolPageViewModel SchoolPage { get; }
    public DownloadDataSelectYearModel SelectYear { get; }

    public SchoolDownloadDataSelectYearViewModel(
        SchoolPageViewModel schoolPage,
        DownloadDataSelectYearModel selectYear
    )
    {
        SchoolPage = schoolPage;
        SelectYear = selectYear;
    }
}
