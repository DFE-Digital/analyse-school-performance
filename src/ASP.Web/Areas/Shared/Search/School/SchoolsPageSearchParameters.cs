using ASP.Core;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolsPageSearchParameters : SearchFormPageParameters
{
    public Func<string, string?> CreateSchoolUrl { get; }

    public SchoolsPageSearchParameters(
        string title, 
        string subTitle, 
        string paginationUrl,
        BreadcrumbTrailViewModel breadcrumbTrailViewModel, 
        string controller, 
        string controllerAction,
        string searchSuggestionUrl,
        Func<string, string?> createSchoolUrl
    ) : base(
        title, 
        subTitle,
        paginationUrl, 
        controller, 
        controllerAction, 
        searchSuggestionUrl,
        Constants.SchoolSearchFormSearchTermInputLabel,
        Constants.SchoolSearchTermInputValidationMessage, 
        breadcrumbTrailViewModel
    )
    {
        CreateSchoolUrl = createSchoolUrl;
    }
}