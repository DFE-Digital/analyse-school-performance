using ASP.Web.Shared;

namespace ASP.Web.Areas.LocalAuthority;

public class DownloadSchoolDataSearchPageViewModel
{
    public PageViewModel Page { get; }
    public string SearchTerm { get; }

    public DownloadSchoolDataSearchPageViewModel(
        PageViewModel page,
        string searchTerm
    )
    {
        Page = page;
        SearchTerm = searchTerm;
    }
}