using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Areas.Shared.Search.School;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared;

public class SchoolsPageViewModel : SchoolSearchFormModel
{
    public string Title { get; }
    public string SubTitle { get; }
    public int TotalCount { get; }
    public PaginationModel? PaginationModel { get; }
    public List<EstablishmentListingModel> EstablishmentListingsModel { get; }
    public BreadcrumbTrailViewModel? Breadcrumbs { get; }

    public SchoolsPageViewModel(
        string title,
        string subTitle,
        int totalCount,
        PaginationModel? paginationModel,
        BreadcrumbTrailViewModel breadcrumbs,
        List<EstablishmentListingModel> establishmentListingsModel,
        string controller, 
        string controllerAction,
        string searchSuggestionUrl) : base(controller, controllerAction, searchSuggestionUrl)
    {
        Title = title;
        SubTitle = subTitle;
        TotalCount = totalCount;
        PaginationModel = paginationModel;
        Breadcrumbs = breadcrumbs;
        EstablishmentListingsModel = establishmentListingsModel;
    }
}