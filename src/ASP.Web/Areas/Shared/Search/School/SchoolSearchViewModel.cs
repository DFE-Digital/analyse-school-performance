using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolSearchViewModel : SchoolSearchFormModel
{
    public List<EstablishmentListingModel> EstablishmentListingsModel { get; }
    public PaginationModel? PaginationModel { get; }
    public string SearchTerm { get; }
    public string? SubTitle { get; }
    public string SearchUrl { get; }
    public int TotalCount { get; }
    public BreadcrumbTrailViewModel Breadcrumbs { get; }

    public SchoolSearchViewModel(List<EstablishmentListingModel> establishmentListingsModel,
        PaginationModel? paginationModel, string searchTerm, int totalCount,
        BreadcrumbTrailViewModel breadcrumbs, string searchUrl, string searchSuggestionUrl,
        string controller, string controllerAction, string? subTitle = null)
        : base(controller, controllerAction, searchSuggestionUrl)
    {
        EstablishmentListingsModel = establishmentListingsModel;
        PaginationModel = paginationModel;
        SearchTerm = searchTerm;
        TotalCount = totalCount;
        Breadcrumbs = breadcrumbs;
        SearchUrl = searchUrl;
        SubTitle = subTitle;
    }
}