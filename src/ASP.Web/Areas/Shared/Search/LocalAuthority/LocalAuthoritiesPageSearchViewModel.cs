using ASP.Core;
using ASP.Web.Areas.Shared.LocalAuthorityListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.LocalAuthority;

public class LocalAuthoritiesPageSearchViewModel : SearchPageViewModel
{
    public List<LocalAuthoritiesListingModel> LocalAuthoritiesListingModel { get; }
    public BreadcrumbTrailViewModel? BreadcrumbTrail { get; }

    public LocalAuthoritiesPageSearchViewModel(
        string title,
        string subTitle,
        int totalCount,
        PaginationModel? paginationModel,
        BreadcrumbTrailViewModel breadcrumbTrail,
        List<LocalAuthoritiesListingModel> localAuthoritiesListingModel,
        string searchSuggestionUrl) 
        : base(
        title,
        subTitle, 
        totalCount,
        searchSuggestionUrl,
        Constants.AlpineComponentLaSearchSuggestions,
        Constants.LaSearchFormSearchTermInputLabel,
        Constants.LaSearchTermInputValidationMessage,
        paginationModel)
    {
        LocalAuthoritiesListingModel = localAuthoritiesListingModel;
        BreadcrumbTrail = breadcrumbTrail;
    }
}