using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search;

public abstract class SearchBaseModel
{
    public string Controller { get; }
    public string ControllerAction { get; }
    public string SearchSuggestionUrl { get; }
    public string InputLabel { get; }
    public string InputValidationMessage { get; }
    public BreadcrumbTrailViewModel? Breadcrumbs { get; }

    protected SearchBaseModel(
        string controller,
        string controllerAction,
        string searchSuggestionUrl,
        string inputLabel,
        string inputValidationMessage,
        BreadcrumbTrailViewModel? breadcrumbs = null)
    {
        Controller = controller;
        ControllerAction = controllerAction;
        SearchSuggestionUrl = searchSuggestionUrl;
        InputLabel = inputLabel;
        InputValidationMessage = inputValidationMessage;
        Breadcrumbs = breadcrumbs;
    }
}
