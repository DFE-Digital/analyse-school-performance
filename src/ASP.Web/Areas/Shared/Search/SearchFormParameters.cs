using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search;

public abstract class SearchFormParameters : SearchBaseModel
{
    public string SearchUrl { get; }

    protected SearchFormParameters(
        string searchUrl,
        string controller,
        string controllerAction,
        string searchSuggestionUrl,
        string inputLabel,
        string inputValidationMessage,
        BreadcrumbTrailViewModel breadcrumbTrail)
        : base(controller, controllerAction, searchSuggestionUrl, inputLabel, inputValidationMessage, breadcrumbTrail)
    {
        SearchUrl = searchUrl;
    }
}