using ASP.Web.Areas.Shared.LocalAuthorityListing;
using ASP.Web.Areas.Shared.Search.Layout.SearchPageLayout;

namespace ASP.Web.Areas.Shared.Search.LocalAuthority;

public class LocalAuthoritySearchPageViewModel
{
    public SearchPageLayoutModel SearchPage { get; }
    public List<LocalAuthoritiesListingModel> LocalAuthorityListings { get; }

    public LocalAuthoritySearchPageViewModel(
        SearchPageLayoutModel searchPage,
        List<LocalAuthoritiesListingModel> localAuthorityListings)
    {
        SearchPage = searchPage;
        LocalAuthorityListings = localAuthorityListings;
    }
}