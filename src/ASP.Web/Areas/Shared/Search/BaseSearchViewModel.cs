using ASP.Web.Areas.Shared.Pagination;

namespace ASP.Web.Areas.Shared.Search;

public abstract class BaseSearchViewModel : SearchBaseModel
{
    public PaginationModel? PaginationModel { get; }
    public int TotalCount { get; }
    public string? SubTitle { get; }

    protected BaseSearchViewModel(
        int totalCount,
        string searchSuggestionUrl,
        PaginationModel? paginationModel,
        string? controller,
        string? controllerAction,
        string? subTitle = null)
        : base(searchSuggestionUrl, controller, controllerAction)
    {
        TotalCount = totalCount;
        PaginationModel = paginationModel;
        SubTitle = subTitle;
    }
}