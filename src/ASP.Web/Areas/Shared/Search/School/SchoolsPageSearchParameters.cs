using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolsPageSearchParameters : SearchFormPageParameters
{
    public Func<string, string?> CreateSchoolUrl { get; }
    public BreadcrumbTrailViewModel? BreadcrumbTrail { get; }

    public SchoolsPageSearchParameters(
        string title, 
        string subTitle, 
        string paginationUrl,
        BreadcrumbTrailViewModel breadcrumbTrail, 
        Func<string, string?> createSchoolUrl,
        string searchSuggestionUrl
    ) : base(
        title, 
        subTitle,
        paginationUrl, 
        null, 
        null, 
        searchSuggestionUrl
    )
    {
        BreadcrumbTrail = breadcrumbTrail;
        CreateSchoolUrl = createSchoolUrl;
    }
}