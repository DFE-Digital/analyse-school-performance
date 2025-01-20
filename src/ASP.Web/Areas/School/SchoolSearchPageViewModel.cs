using ASP.Web.Features.Search;
using ASP.Web.Shared;

namespace ASP.Web.Areas.School;

public class SchoolSearchPageViewModel
{
    public PageViewModel Page { get; }
    public SearchViewModel Search { get; }
    public List<EstablishmentListingViewModel> EstablishmentListings { get; }

    public SchoolSearchPageViewModel(
        PageViewModel page,
        SearchViewModel search,
        List<EstablishmentListingViewModel> establishmentListings)
    {
        Page = page;
        Search = search;
        EstablishmentListings = establishmentListings;
    }
}