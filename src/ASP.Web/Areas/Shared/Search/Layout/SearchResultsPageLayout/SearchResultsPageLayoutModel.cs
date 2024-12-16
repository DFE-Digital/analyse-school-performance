using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Areas.Shared.Search.Partial.SearchForm;
using ASP.Web.Shared;

namespace ASP.Web.Areas.Shared.Search.Layout.SearchResultsPageLayout;

public class SearchResultsPageLayoutModel
{
    public PageViewModel Page { get; }
    public SearchFormViewModel SearchForm { get; }
    public string SearchTerm { get; }
    public string SearchUrl { get; }
    public PaginationModel? Pagination { get; }

    public SearchResultsPageLayoutModel(
        PageViewModel page,
        SearchFormViewModel searchForm,
        string searchTerm,
        string searchUrl,
        PaginationModel? pagination = null)
    {
        Page = page;
        SearchForm = searchForm;
        SearchTerm = searchTerm;
        SearchUrl = searchUrl;
        Pagination = pagination;
    }
}