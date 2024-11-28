namespace ASP.Web.Areas.Shared.Search;

public class SearchFormModel : SearchBaseModel
{
    public SearchFormModel(
        string controller,
        string controllerAction,
        string searchSuggestionUrl,
        string inputLabel,
        string inputValidationMessage)
        : base(controller, controllerAction, searchSuggestionUrl, inputLabel, inputValidationMessage)
    {
    }
}