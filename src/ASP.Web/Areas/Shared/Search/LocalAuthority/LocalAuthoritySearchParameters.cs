using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.LocalAuthority;

public class LocalAuthoritySearchParameters : SearchFormParameters
{
    public BreadcrumbTrailViewModel? BreadcrumbTrail { get; }

    public LocalAuthoritySearchParameters(
        string searchUrl,
        string searchSuggestionUrl,
        string controller,
        string controllerAction,
        BreadcrumbTrailViewModel breadcrumbTrail)
        : base(searchUrl,
            controller,
            controllerAction,
            searchSuggestionUrl)
    {
        BreadcrumbTrail = breadcrumbTrail;
    }
}