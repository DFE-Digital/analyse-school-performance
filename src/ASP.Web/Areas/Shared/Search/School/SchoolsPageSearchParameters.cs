using ASP.Core;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolsPageSearchParameters : SearchFormPageParameters
{
    public SchoolsPageSearchParameters(string title, string subTitle, string paginationUrl,
        BreadcrumbTrailViewModel breadcrumbTrailViewModel, string controller, string controllerAction,
        string searchSuggestionUrl) : base(title, subTitle,
        paginationUrl, controller, controllerAction, searchSuggestionUrl,
        Constants.SchoolSearchFormSearchTermInputLabel,
        Constants.SchoolSearchTermInputValidationMessage, breadcrumbTrailViewModel)
    {
    }
}