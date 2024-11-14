using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolSearchParameters
{
    public string SearchUrl { get; }
    public string SearchSuggestionUrl { get; }
    public string Controller { get; }
    public string ControllerAction { get; }
    public BreadcrumbTrailViewModel BreadcrumbTrail { get; }

    public SchoolSearchParameters(
        string searchUrl,
        string searchSuggestionUrl,
        string controller,
        string controllerAction,
        BreadcrumbTrailViewModel breadcrumbTrail)
    {
        SearchUrl = searchUrl;
        SearchSuggestionUrl = searchSuggestionUrl;
        Controller = controller;
        ControllerAction = controllerAction;
        BreadcrumbTrail = breadcrumbTrail;
    }
}