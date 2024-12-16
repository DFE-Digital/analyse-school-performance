using ASP.Web.Areas.Shared.LocalAuthorityListing;
using ASP.Web.Areas.Shared.Search.Layout.SearchResultsPageLayout;

namespace ASP.Web.Areas.Shared.Search.LocalAuthority;

public class LocalAuthoritySearchResultsPageViewModel
{
    public SearchResultsPageLayoutModel SearchResultsPage { get; }
    public List<LocalAuthoritiesListingModel> LocalAuthorityListings { get; }

    public LocalAuthoritySearchResultsPageViewModel(
        SearchResultsPageLayoutModel searchResultsPage,
        List<LocalAuthoritiesListingModel> localAuthorityListings)
    {
        SearchResultsPage = searchResultsPage;
        LocalAuthorityListings = localAuthorityListings;
    }
}