using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.Templating;

namespace ASP.Web.Areas.Search;

public class SearchViewModel
{
    public List<EstablishmentSearchResultsModel> SearchResults { get; }
    public PaginationModel? PaginationModel { get; }
    public string SearchTerm { get; }
    public int TotalCount { get; }
    public BreadcrumbViewModel Breadcrumbs { get; }

    public SearchViewModel(List<EstablishmentSearchResultsModel> searchResults, PaginationModel? paginationModel, string searchTerm, int totalCount, BreadcrumbViewModel breadcrumbs)
    {
        SearchResults = searchResults;
        PaginationModel = paginationModel;
        SearchTerm = searchTerm;
        TotalCount = totalCount;
        Breadcrumbs = breadcrumbs;
    }
}