using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search;

public abstract class BaseSearchViewModel : SearchBaseModel
{
    public PaginationModel? PaginationModel { get; }
    public int TotalCount { get; }
    public string? SubTitle { get; }

    protected BaseSearchViewModel(
        int totalCount,
        PaginationModel? paginationModel,
        string controller,
        string controllerAction,
        string searchSuggestionUrl,
        string inputLabel,
        string inputValidationMessage,
        BreadcrumbTrailViewModel breadcrumbs,
        string? subTitle = null)
        : base(controller, controllerAction, searchSuggestionUrl, inputLabel, inputValidationMessage, breadcrumbs)
    {
        TotalCount = totalCount;
        PaginationModel = paginationModel;
        SubTitle = subTitle;
    }
}
