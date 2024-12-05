namespace ASP.Web.Areas.Shared.Search;

public class SearchFormModel : SearchBaseModel
{
    public string AlpineComponentName { get; }
    public string InputLabel { get; }
    public string InputValidationMessage { get; }

    public SearchFormModel(
        string searchSuggestionUrl,
        string alpineComponentName,
        string inputLabel,
        string inputValidationMessage,
        string? controller = null,
        string? controllerAction = null)
        : base(
            searchSuggestionUrl,
            controller,
            controllerAction
        )
    {
        AlpineComponentName = alpineComponentName;
        InputLabel = inputLabel;
        InputValidationMessage = inputValidationMessage;
    }
}