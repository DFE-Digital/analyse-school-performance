using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search;

public abstract class SearchViewModel : BaseSearchViewModel
{
    public string SearchTerm { get; }
    public string SearchUrl { get; }

    protected SearchViewModel(
        string searchTerm,
        string searchUrl,
        int totalCount,
        PaginationModel? paginationModel,
        string controller,
        string controllerAction,
        string searchSuggestionUrl,
        string inputLabel,
        string inputValidationMessage,
        BreadcrumbTrailViewModel breadcrumbs,
        string? subTitle = null)
        : base(totalCount, paginationModel, controller, controllerAction, searchSuggestionUrl,
            inputLabel, inputValidationMessage, breadcrumbs, subTitle)
    {
        SearchTerm = searchTerm;
        SearchUrl = searchUrl;
    }
}