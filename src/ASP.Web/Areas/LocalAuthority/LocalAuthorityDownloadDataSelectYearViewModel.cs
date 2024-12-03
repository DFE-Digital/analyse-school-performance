using ASP.Web.Areas.Shared.DownloadData.SelectYear;
using ASP.Web.Shared;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthorityDownloadDataSelectYearViewModel
{
    public PageViewModel Page { get; }
    public DownloadDataSelectYearModel SelectYear { get; }

    public LocalAuthorityDownloadDataSelectYearViewModel(
        PageViewModel page,
        DownloadDataSelectYearModel selectYear
    )
    {
        Page = page;
        SelectYear = selectYear;
    }
}
