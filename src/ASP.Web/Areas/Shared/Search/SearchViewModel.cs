using ASP.Web.Areas.Shared.Pagination;

namespace ASP.Web.Areas.Shared.Search;

public abstract class SearchViewModel : BaseSearchViewModel
{
    public string SearchTerm { get; }
    public string SearchUrl { get; }
    public string AlpineComponentName { get; }
    public string InputLabel { get; }
    public string InputValidationMessage { get; }

    protected SearchViewModel(
        string searchTerm,
        string searchUrl,
        int totalCount,
        PaginationModel? paginationModel,
        string controller,
        string controllerAction,
        string searchSuggestionUrl,
        string alpineComponentName,
        string inputLabel,
        string inputValidationMessage,
        string? subTitle = null)
        : base(totalCount,
            searchSuggestionUrl,
            paginationModel,
            controller,
            controllerAction,
            subTitle)
    {
        SearchTerm = searchTerm;
        SearchUrl = searchUrl;
        AlpineComponentName = alpineComponentName;
        InputLabel = inputLabel;
        InputValidationMessage = inputValidationMessage;
    }
}