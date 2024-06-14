using ASP.Web.Areas.Shared.Pagination;

namespace ASP.Web.Areas.Search;

public class SearchViewModel
{
    public List<EstablishmentSearchResultsModel> SearchResults { get; set; } = new List<EstablishmentSearchResultsModel>();
    public PaginationModel PaginationModel { get; set; } = new PaginationModel();
    public string SearchTerm { get; set; } = string.Empty;
    public int TotalCount { get; set; }

}