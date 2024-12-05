using ASP.Core;
using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolsPageSearchViewModel : SearchPageViewModel
{
    public List<EstablishmentListingModel> EstablishmentListingsModel { get; }
    public BreadcrumbTrailViewModel? BreadcrumbTrail { get; }

    public SchoolsPageSearchViewModel(string title,
        string subTitle,
        int totalCount,
        PaginationModel? paginationModel,
        BreadcrumbTrailViewModel breadcrumbTrail,
        List<EstablishmentListingModel> establishmentListingsModel,
        string searchSuggestionUrl)
        : base(title,
            subTitle,
            totalCount,
            searchSuggestionUrl,
            Constants.AlpineComponentSchoolSearchSuggestions,
            Constants.SchoolSearchFormSearchTermInputLabel,
            Constants.SchoolSearchTermInputValidationMessage,
            paginationModel)
    {
        EstablishmentListingsModel = establishmentListingsModel;
        BreadcrumbTrail = breadcrumbTrail;
    }
}