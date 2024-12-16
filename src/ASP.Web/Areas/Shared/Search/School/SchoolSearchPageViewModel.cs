using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Search.Layout.SearchPageLayout;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolSearchPageViewModel
{
    public SearchPageLayoutModel SearchPage { get; }
    public List<EstablishmentListingModel> EstablishmentListings { get; }

    public SchoolSearchPageViewModel(
        SearchPageLayoutModel searchPage,
        List<EstablishmentListingModel> establishmentListings)
    {
        SearchPage = searchPage;
        EstablishmentListings = establishmentListings;
    }
}