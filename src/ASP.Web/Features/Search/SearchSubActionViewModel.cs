using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Features.Search;

public record SearchSubActionViewModel<TSearchResultViewModel>(
    string PageTitle,
    string PageSubtitle,
    BreadcrumbTrailViewModel BreadcrumbTrail,
    SearchViewModel Search,
    List<TSearchResultViewModel> SearchResults
)
{
}