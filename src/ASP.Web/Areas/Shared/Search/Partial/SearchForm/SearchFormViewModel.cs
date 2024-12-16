namespace ASP.Web.Areas.Shared.Search.Partial.SearchForm;

public class SearchFormViewModel
{
    public string SearchSuggestionUrl { get; }
    public string AlpineComponentName { get; }
    public string InputLabel { get; }
    public string InputValidationMessage { get; }

    public SearchFormViewModel(
        string searchSuggestionUrl,
        string alpineComponentName,
        string inputLabel,
        string inputValidationMessage)
    {
        SearchSuggestionUrl = searchSuggestionUrl;
        AlpineComponentName = alpineComponentName;
        InputLabel = inputLabel;
        InputValidationMessage = inputValidationMessage;
    }
}