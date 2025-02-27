using ASP.Web.Areas.School;
using ASP.Web.Features.Search;
using ASP.Web.Shared;

namespace ASP.Web.Areas.LocalAuthority;

public class DownloadSchoolDataSearchPageViewModel
{
    public PageViewModel Page { get; }
    public SearchViewModel Search { get; }
    public List<SchoolListingViewModel> SchoolListings { get; }

    public DownloadSchoolDataSearchPageViewModel(
        PageViewModel page,
        SearchViewModel search,
        List<SchoolListingViewModel> schoolListings)
    {
        Page = page;
        Search = search;
        SchoolListings = schoolListings;
    }
}