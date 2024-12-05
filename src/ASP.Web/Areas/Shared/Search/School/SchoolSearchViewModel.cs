using ASP.Core;
using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;

namespace ASP.Web.Areas.Shared.Search.School;

public class SchoolSearchViewModel : SearchViewModel
{
    public List<EstablishmentListingModel> EstablishmentListingsModel { get; }
    public Func<string, string?> CreateSchoolUrl { get; }
    public BreadcrumbTrailViewModel? BreadcrumbTrail { get; }

    public SchoolSearchViewModel(
        List<EstablishmentListingModel> establishmentListingsModel,
        PaginationModel? paginationModel, 
        string searchTerm, 
        int totalCount,
        BreadcrumbTrailViewModel breadcrumbTrail, 
        string controller, 
        string controllerAction, 
        string searchUrl,
        string searchSuggestionUrl,
        Func<string, string?> createSchoolUrl,
        string? subtitle = null
    ) : base(
        searchTerm, 
        searchUrl, 
        totalCount, 
        paginationModel, 
        controller, 
        controllerAction, 
        searchSuggestionUrl,
        Constants.AlpineComponentSchoolSearchSuggestions,
        Constants.SchoolSearchFormSearchTermInputLabel,
        Constants.SchoolSearchTermInputValidationMessage,
        subtitle)
    {
        EstablishmentListingsModel = establishmentListingsModel;
        CreateSchoolUrl = createSchoolUrl;
        BreadcrumbTrail = breadcrumbTrail;
    }
}