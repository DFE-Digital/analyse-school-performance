using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Search.Layout.SearchResultsPageLayout;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolSearchResultsPageViewModel
{
    public SearchResultsPageLayoutModel SearchResultsPage { get; }
    public List<EstablishmentListingModel> EstablishmentListings { get; }

    public SchoolSearchResultsPageViewModel(
        SearchResultsPageLayoutModel searchResultsPage,
        List<EstablishmentListingModel> establishmentListings)
    {
        SearchResultsPage = searchResultsPage;
        EstablishmentListings = establishmentListings;
    }
}