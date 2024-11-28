using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolsPageSearchViewModel : SearchPageViewModel
{
    public List<EstablishmentListingModel> EstablishmentListingsModel { get; }

    public SchoolsPageSearchViewModel(string title, string subTitle, int totalCount, PaginationModel? paginationModel,
        BreadcrumbTrailViewModel breadcrumbs, List<EstablishmentListingModel> establishmentListingsModel,
        string controller, string controllerAction, string searchSuggestionUrl,
        string inputLabel, string inputValidationMessage) : base(title, subTitle, totalCount, paginationModel,
        controller, controllerAction, searchSuggestionUrl, inputLabel, inputValidationMessage, breadcrumbs)
    {
        EstablishmentListingsModel = establishmentListingsModel;
    }
}