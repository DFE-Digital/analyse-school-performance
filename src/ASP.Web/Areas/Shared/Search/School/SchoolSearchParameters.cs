using ASP.Core;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolSearchParameters : SearchFormParameters
{
    public Func<string, string?> CreateSchoolUrl { get; }

    public SchoolSearchParameters(
		string searchUrl, 
		string searchSuggestionUrl, 
		string controller,
        string controllerAction, 
		BreadcrumbTrailViewModel breadcrumbTrail,
		Func<string, string?> createSchoolUrl
	) : base(
		searchUrl, 
		controller,
		controllerAction,
		searchSuggestionUrl, 
		Constants.SchoolSearchFormSearchTermInputLabel,
		Constants.SchoolSearchTermInputValidationMessage,
		breadcrumbTrail
    )
    {
        CreateSchoolUrl = createSchoolUrl;
    }
}