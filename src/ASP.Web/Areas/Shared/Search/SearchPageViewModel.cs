using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search;

public abstract class SearchPageViewModel : BaseSearchViewModel
{
    public string Title { get; }

    protected SearchPageViewModel(
        string title,
        string subTitle,
        int totalCount,
        PaginationModel? paginationModel,
        string controller,
        string controllerAction,
        string searchSuggestionUrl,
        string inputLabel,
        string inputValidationMessage,
        BreadcrumbTrailViewModel breadcrumbs)
        : base(totalCount, paginationModel, controller, controllerAction, searchSuggestionUrl,
            inputLabel, inputValidationMessage, breadcrumbs, subTitle)
    {
        Title = title;
    }
}