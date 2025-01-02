using ASP.Web.Shared;

namespace ASP.Web.Areas.LocalAuthority;

public class DownloadSchoolDataSearchResultsPageViewModel
{
    public PageViewModel Page { get; }
    public string SearchTerm { get; }

    public DownloadSchoolDataSearchResultsPageViewModel(
        PageViewModel page,
        string searchTerm
    )
    {
        Page = page;
        SearchTerm = searchTerm;
    }
}