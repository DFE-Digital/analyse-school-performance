using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Search;

public class SearchViewModel
{
    public List<EstablishmentListingModel> EstablishmentListingsModel { get; }
    public PaginationModel? PaginationModel { get; }
    public string SearchTerm { get; }
    public int TotalCount { get; }
    public BreadcrumbTrailViewModel Breadcrumbs { get; }

    public SearchViewModel(List<EstablishmentListingModel> establishmentListingsModel,
        PaginationModel? paginationModel,
        string searchTerm, int totalCount,
        BreadcrumbTrailViewModel breadcrumbs)
    {
        EstablishmentListingsModel = establishmentListingsModel;
        PaginationModel = paginationModel;
        SearchTerm = searchTerm;
        TotalCount = totalCount;
        Breadcrumbs = breadcrumbs;
    }
}