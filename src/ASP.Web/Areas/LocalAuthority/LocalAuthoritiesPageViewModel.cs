using ASP.Application.UseCases.LocalAuthorities.DTO;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.LocalAuthority;

public class LocalAuthoritiesPageViewModel
{
    public string Title { get; }
    public int TotalCount { get; }
    public PaginationModel? PaginationModel { get; }
    public IEnumerable<LocalAuthorityDTO> Results { get; }
    public BreadcrumbTrailViewModel? Breadcrumbs { get; }

    public LocalAuthoritiesPageViewModel(string title, int totalCount, PaginationModel? paginationModel,
        IEnumerable<LocalAuthorityDTO> results, BreadcrumbTrailViewModel? breadcrumbs)
    {
        Title = title;
        TotalCount = totalCount;
        PaginationModel = paginationModel;
        Results = results;
        Breadcrumbs = breadcrumbs;
    }
}