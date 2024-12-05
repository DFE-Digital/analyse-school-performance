using ASP.Web.Areas.Shared.Pagination;

namespace ASP.Web.Areas.Shared.Search;

public abstract class SearchPageViewModel : BaseSearchViewModel
{
    public string Title { get; }
    public string AlpineComponentName { get; }
    public string InputLabel { get; }
    public string InputValidationMessage { get; }

    protected SearchPageViewModel(
        string title,
        string subTitle,
        int totalCount,
        string searchSuggestionUrl,
        string alpineComponentName,
        string inputLabel,
        string inputValidationMessage,
        PaginationModel? paginationModel,
        string? controller = null,
        string? controllerAction = null)
        : base(totalCount,
            searchSuggestionUrl,
            paginationModel,
            controller,
            controllerAction,
            subTitle)
    {
        Title = title;
        AlpineComponentName = alpineComponentName;
        InputLabel = inputLabel;
        InputValidationMessage = inputValidationMessage;
    }
}