using ASP.Web.Areas.Shared.LocalAuthorityListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.LocalAuthority;

public class LocalAuthoritiesPageSearchViewModel : SearchPageViewModel
{
    public List<LocalAuthoritiesListingModel> LocalAuthoritiesListingModel { get; }

    public LocalAuthoritiesPageSearchViewModel(string title, string subTitle, int totalCount,
        PaginationModel? paginationModel, BreadcrumbTrailViewModel breadcrumbs,
        List<LocalAuthoritiesListingModel> localAuthoritiesListingModel, string controller,
        string controllerAction, string searchSuggestionUrl, string inputLabel, string inputValidationMessage) : base(
        title, subTitle, totalCount, paginationModel, controller, controllerAction, searchSuggestionUrl,
        inputLabel, inputValidationMessage, breadcrumbs)
    {
        LocalAuthoritiesListingModel = localAuthoritiesListingModel;
    }
}