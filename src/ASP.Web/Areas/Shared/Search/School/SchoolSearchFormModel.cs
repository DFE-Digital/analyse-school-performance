namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolSearchFormModel
{
    public string Controller { get; }
    public string ControllerAction { get; }
    public string SearchSuggestionUrl { get; }

    public SchoolSearchFormModel(string controller, string controllerAction, string searchSuggestionUrl)
    {
        Controller = controller;
        ControllerAction = controllerAction;
        SearchSuggestionUrl = searchSuggestionUrl;
    }
}