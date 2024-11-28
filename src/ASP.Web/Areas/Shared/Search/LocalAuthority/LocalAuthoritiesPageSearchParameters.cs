using ASP.Core;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.LocalAuthority;

public class LocalAuthoritiesPageSearchParameters : SearchFormPageParameters
{
    public LocalAuthoritiesPageSearchParameters(string title, string subTitle, string paginationUrl,
        BreadcrumbTrailViewModel breadcrumbTrailViewModel, string controller, string controllerAction,
        string searchSuggestionUrl) : base(title, subTitle,
        paginationUrl, controller, controllerAction, searchSuggestionUrl,
        Constants.LaSearchFormSearchTermInputLabel,
        Constants.LaSearchTermInputValidationMessage, breadcrumbTrailViewModel)
    {
    }
}