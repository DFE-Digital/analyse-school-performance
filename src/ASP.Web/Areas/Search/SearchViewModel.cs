using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Search;

public class SearchViewModel
{
    public List<EstablishmentSearchResultsModel> SearchResults { get; }
    public PaginationModel? PaginationModel { get; }
    public string SearchTerm { get; }
    public int TotalCount { get; }
    public BreadcrumbTrailViewModel Breadcrumbs { get; }

    public SearchViewModel(List<EstablishmentSearchResultsModel> searchResults, PaginationModel? paginationModel, string searchTerm, int totalCount, BreadcrumbTrailViewModel breadcrumbs)
    {
        SearchResults = searchResults;
        PaginationModel = paginationModel;
        SearchTerm = searchTerm;
        TotalCount = totalCount;
        Breadcrumbs = breadcrumbs;
    }
}