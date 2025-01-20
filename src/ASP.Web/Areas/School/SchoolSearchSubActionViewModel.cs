using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Features.Search;

namespace ASP.Web.Areas.School
{
    public record SchoolSearchSubActionViewModel(
        string PageTitle,
        string PageSubtitle,
        BreadcrumbTrailViewModel BreadcrumbTrail,
        SearchViewModel Search,
        List<EstablishmentListingViewModel> Establishments
    )
    {
    }
}