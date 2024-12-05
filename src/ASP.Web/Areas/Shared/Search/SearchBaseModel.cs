namespace ASP.Web.Areas.Shared.Search;

public abstract class SearchBaseModel
{
    public string? Controller { get; }
    public string? ControllerAction { get; }
    public string SearchSuggestionUrl { get; }


    protected SearchBaseModel(
        string searchSuggestionUrl,
        string? controller,
        string? controllerAction)
    {
        Controller = controller;
        ControllerAction = controllerAction;
        SearchSuggestionUrl = searchSuggestionUrl;
    }
}