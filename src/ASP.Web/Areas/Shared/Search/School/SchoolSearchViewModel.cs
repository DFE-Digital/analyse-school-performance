using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolSearchViewModel : SearchViewModel
{
    public List<EstablishmentListingModel> EstablishmentListingsModel { get; }

    public SchoolSearchViewModel(List<EstablishmentListingModel> establishmentListingsModel,
        PaginationModel? paginationModel, string searchTerm, int totalCount,
        BreadcrumbTrailViewModel breadcrumbs, string controller, string controllerAction, string searchUrl,
        string searchSuggestionUrl,
        string inputLabel, string inputValidationMessage, string? subTitle = null) : base(
         searchTerm, searchUrl, totalCount, paginationModel, controller, controllerAction, searchSuggestionUrl,
        inputLabel, inputValidationMessage, breadcrumbs, subTitle)
    {
        EstablishmentListingsModel = establishmentListingsModel;
    }
}