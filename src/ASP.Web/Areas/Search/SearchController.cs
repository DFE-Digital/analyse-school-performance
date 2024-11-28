using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Core;
using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Helpers;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Areas.School;
using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Areas.Shared.Search;
using ASP.Web.Areas.Shared.Search.School;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.Search;

[Area("Search")]
[Route("search")]
[ServiceFilter<TermsOfUseActionFilter>]
[Authorize(Policy = Policy.AccessToSearch)]
public class SearchController : Controller
{
    private readonly IAspApiClient _api;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILocalAuthorityRepository _localAuthorityRepository;
    private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;

    public SearchController(
        IAspApiClient api,
        IHostEnvironment hostEnvironment,
        ILocalAuthorityRepository localAuthorityRepository,
        IMultiAcademyTrustRepository multiAcademyTrustRepository)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        _localAuthorityRepository = localAuthorityRepository ??
                                    throw new ArgumentNullException(nameof(localAuthorityRepository));
        _multiAcademyTrustRepository = multiAcademyTrustRepository ??
                                       throw new ArgumentNullException(nameof(multiAcademyTrustRepository));
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(SearchParams searchParams)
    {
        if (!Request.Query.Keys.Any(k =>
                k.Equals(nameof(searchParams.Search), StringComparison.InvariantCultureIgnoreCase)))
        {
            return View(nameof(Index), GetEmptySchoolSearchViewModel());
        }

        if (!ModelState.IsValid)
        {
            return View(nameof(Index), GetEmptySchoolSearchViewModel());
        }

        var pageNumber = PageHelper.ParsePageNumber(searchParams.Page);

        if (string.IsNullOrEmpty(searchParams.Search))
        {
            ModelState.AddModelError(nameof(searchParams.Search), Constants.SchoolSearchTermInputValidationMessage);
            return View(nameof(Index), GetEmptySchoolSearchViewModel());
        }
        
        var searchResult = await PerformSearchBasedOnUserRole(searchParams, pageNumber);

        return searchResult.ToActionResult(RedirectToSchoolLandingPageIfSingleResult, _hostEnvironment);
    }

    [HttpGet("suggestions")]
    public async Task<IActionResult> SearchSuggestions(SearchParams searchParams)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index));
        }
        
        var searchResult = await PerformSearchSuggestionsBasedOnUserRole(searchParams);

        return searchResult.ToActionResult(Json, _hostEnvironment);
    }
    
    private SchoolSearchViewModel GetEmptySchoolSearchViewModel()
    {
        return DefaultViewModel(
            new ScopedSearchResultsPage<EstablishmentListingDTO>());
    }
    
    private SchoolSearchViewModel DefaultViewModel(ScopedSearchResultsPage<EstablishmentListingDTO> result)
    {
        var breadcrumbTrail = new BreadcrumbTrailViewModel($"Search results for \"{result.SearchTerm}\"")
            .AddBreadcrumb("Search", "/search");
        
        return new SchoolSearchViewModel(
            EstablishmentListingModel
                .FromEstablishmentListingDto(result.Results),
            new PaginationModel(
                Url.Action(nameof(Index), new { search = result.SearchTerm }) ?? "",
                result.Page,
                result.TotalResults,
                result.ResultsPerPage,
                "school or college",
                "schools or colleges"
            ),
            result.SearchTerm,
            result.TotalResults,
            breadcrumbTrail,
            "Search",
            nameof(Index),
            "/search/",
            "/search/suggestions/",
            Constants.SchoolSearchFormSearchTermInputLabel,
            Constants.SchoolSearchTermInputValidationMessage
        );
    }

    private SchoolSearchViewModel NoResultsViewModel(SearchParams searchParams)
    {
        var breadcrumbTrail = new BreadcrumbTrailViewModel($"We found no matches for \"{searchParams.Search}\"")
                .AddBreadcrumb("Search", "/search");

        return new SchoolSearchViewModel(
            establishmentListingsModel: new List<EstablishmentListingModel>(),
            paginationModel: null,
            searchTerm: searchParams.Search ?? "",
            totalCount: 0,
            breadcrumbs: breadcrumbTrail,
            controller: "Search",
            controllerAction: nameof(Index),
            searchUrl: "/search/",
            searchSuggestionUrl: "/search/suggestions/",
            Constants.SchoolSearchFormSearchTermInputLabel,
            Constants.SchoolSearchTermInputValidationMessage
        );
    }

    private IActionResult RedirectToSchoolLandingPageIfSingleResult(SchoolSearchViewModel model)
    {
        if (model.TotalCount == 1)
        {
            return RedirectToAction(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                new { area = "School", urn = model.EstablishmentListingsModel.FirstOrDefault()!.Urn });
        }

        return View("SchoolSearchResults", model);
    }

    private Task<Result<SchoolSearchViewModel>> PerformSearchBasedOnUserRole(SearchParams searchParams,
        int pageNumber)
    {
        return
            from scopeInfo in Scope.GetScopeInfoForRole(User, _localAuthorityRepository, _multiAcademyTrustRepository)
            from result in PerformEstablishmentSearch(searchParams, scopeInfo, pageNumber)
            select result;
    }

    private Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>>
        PerformSearchSuggestionsBasedOnUserRole(SearchParams searchParams)
    {
        return
            from scopeInfo in Scope.GetScopeInfoForRole(User, _localAuthorityRepository, _multiAcademyTrustRepository)
            from result in PerformEstablishmentSearchSuggestions(searchParams, scopeInfo)
            select result;
    }
    
    private Task<Result<SchoolSearchViewModel>> PerformEstablishmentSearch(SearchParams searchParams, ScopeInfo scopeInfo, int pageNumber)
    {
        var estabSearchRequest = new EstablishmentSearchRequest(
            searchParams.Search ?? "",
            scopeInfo.ScopeType,
            scopeInfo.ScopeId,
            Optional<int>.Some(pageNumber),
            Optional<int>.Some(Constants.SearchResultPageSize)
        );

        var model =
            from searchResults in _api.EstablishmentSearch(estabSearchRequest)
            select DefaultViewModel(searchResults);
        
        return model
            .DefaultIf(e => e is NotFoundError, NoResultsViewModel(searchParams));
    }

    private Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>> PerformEstablishmentSearchSuggestions(
        SearchParams searchParams, ScopeInfo scopeInfo)
    {
        var request = new EstablishmentSearchSuggestionsRequest(
            searchParams.Search ?? "",
            scopeInfo.ScopeType,
            scopeInfo.ScopeId,
            Optional<int>.None
        );

        return _api.EstablishmentSearchSuggestions(request);
    }
}