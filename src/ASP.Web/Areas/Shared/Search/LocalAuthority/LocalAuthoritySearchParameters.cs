using ASP.Core;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.LocalAuthority;

public class LocalAuthoritySearchParameters : SearchFormParameters
{
    public LocalAuthoritySearchParameters(string searchUrl, string searchSuggestionUrl, string controller,
        string controllerAction, BreadcrumbTrailViewModel breadcrumbTrail)
        : base(searchUrl, controller, controllerAction, searchSuggestionUrl,
            Constants.LaSearchFormSearchTermInputLabel,
            Constants.LaSearchTermInputValidationMessage, breadcrumbTrail)
    {
    }
}