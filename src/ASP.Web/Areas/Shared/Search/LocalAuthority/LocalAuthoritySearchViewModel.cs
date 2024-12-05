using ASP.Core;
using ASP.Web.Areas.Shared.LocalAuthorityListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.LocalAuthority;

public class LocalAuthoritySearchViewModel : SearchViewModel
{
    public List<LocalAuthoritiesListingModel> LocalAuthoritiesListingModel { get; }
    public BreadcrumbTrailViewModel? BreadcrumbTrail { get; }

    public LocalAuthoritySearchViewModel(List<LocalAuthoritiesListingModel> localAuthoritiesListingModel,
        PaginationModel? paginationModel,
        string searchTerm,
        int totalCount,
        BreadcrumbTrailViewModel breadcrumbTrail,
        string searchUrl,
        string searchSuggestionUrl,
        string controller,
        string controllerAction,
        string? subTitle = null)
        : base(searchTerm,
            searchUrl,
            totalCount,
            paginationModel,
            controller,
            controllerAction,
            searchSuggestionUrl,
            Constants.AlpineComponentLaSearchSuggestions,
            Constants.LaSearchFormSearchTermInputLabel,
            Constants.LaSearchTermInputValidationMessage,
            subTitle)
    {
        LocalAuthoritiesListingModel = localAuthoritiesListingModel;
        BreadcrumbTrail = breadcrumbTrail;
    }
}