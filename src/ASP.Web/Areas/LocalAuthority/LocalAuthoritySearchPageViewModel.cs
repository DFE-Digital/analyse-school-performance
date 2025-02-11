using ASP.Web.Features.Search;
using ASP.Web.Shared;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthoritySearchPageViewModel
{
    public PageViewModel Page { get; }
    public SearchViewModel Search { get; }
    public List<LocalAuthorityListingViewModel> LocalAuthorityListings { get; }

    public LocalAuthoritySearchPageViewModel(
        PageViewModel page,
        SearchViewModel search,
        List<LocalAuthorityListingViewModel> localAuthorityListings)
    {
        Page = page;
        Search = search;
        LocalAuthorityListings = localAuthorityListings;
    }
}