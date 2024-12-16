using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Areas.Shared.Search.Partial.SearchForm;
using ASP.Web.Shared;

namespace ASP.Web.Areas.Shared.Search.Layout.SearchPageLayout;

public class SearchPageLayoutModel
{
    public PageViewModel Page { get; }
    public SearchFormViewModel SearchForm { get; }
    public PaginationModel? Pagination { get; }

    public SearchPageLayoutModel(
        PageViewModel page,
        SearchFormViewModel searchForm,
        PaginationModel? pagination)
    {
        Page = page;
        SearchForm = searchForm;
        Pagination = pagination;
    }
}