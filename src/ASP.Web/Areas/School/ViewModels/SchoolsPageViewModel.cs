using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.School.ViewModels;

public class SchoolsPageViewModel
{
    public string Title { get; }
    public int TotalCount { get; }
    public PaginationModel? PaginationModel { get; }
    public List<EstablishmentListingModel> EstablishmentListingsModel { get; }
    public BreadcrumbTrailViewModel? Breadcrumbs { get; }

    public SchoolsPageViewModel(
        string title,
        int totalCount,
        PaginationModel? paginationModel,
        BreadcrumbTrailViewModel breadcrumbs,
        List<EstablishmentListingModel> establishmentListingsModel)
    {
        Title = title;
        TotalCount = totalCount;
        PaginationModel = paginationModel;
        Breadcrumbs = breadcrumbs;
        EstablishmentListingsModel = establishmentListingsModel;
    }
}