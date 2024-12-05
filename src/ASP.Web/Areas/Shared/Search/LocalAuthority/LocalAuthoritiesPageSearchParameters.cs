using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.LocalAuthority;

public class LocalAuthoritiesPageSearchParameters : SearchFormPageParameters
{
    public BreadcrumbTrailViewModel? BreadcrumbTrail { get; }
    public LocalAuthoritiesPageSearchParameters(
        string title,
        string subTitle,
        string paginationUrl,
        BreadcrumbTrailViewModel breadcrumbTrail,
        string searchSuggestionUrl)
        : base(title,
            subTitle,
            paginationUrl,
            null,
            null,
            searchSuggestionUrl)
    {
        BreadcrumbTrail = breadcrumbTrail;
    }
}