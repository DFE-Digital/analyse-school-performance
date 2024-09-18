using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Core;
using ASP.Core.Authorization;
using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Areas.School;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;
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
            return View(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            return View(nameof(Index));
        }

        var pageNumber = ParsePageNumber(searchParams.Page);

        if (string.IsNullOrEmpty(searchParams.Search))
        {
            ModelState.AddModelError(nameof(searchParams.Search), Constants.SchoolSearchTermShortValidationMessage);
            return View(nameof(Index));
        }

        var userRole = User.Role();
        var searchResult = await PerformSearchBasedOnUserRole(userRole!, searchParams, pageNumber);

        return searchResult.ToActionResult(RedirectToSchoolLandingPageIfSingleResult, _hostEnvironment);
    }

    [HttpGet("suggestions")]
    public async Task<IActionResult> SearchSuggestions(SearchParams searchParams)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index));
        }

        var userRole = User.Role();
        var searchResult = await PerformSearchSuggestionsBasedOnUserRole(userRole!, searchParams);

        return searchResult.ToActionResult(Json, _hostEnvironment);
    }

    private int ParsePageNumber(string? page)
    {
        return int.TryParse(page, out int intValue) && intValue >= 1 ? intValue : 1;
    }

    private SearchViewModel DefaultViewModel(SearchResultsPage<EstablishmentListingDTO> result)
    {
        var breadcrumbTrail = new BreadcrumbTrailViewModel($"Search results for \"{result.SearchTerm}\"")
            .AddBreadcrumb("Search", "/search");

        return new SearchViewModel(
            EstablishmentSearchResultsModel.FromEstablishmentDetails(result.Results),
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
            breadcrumbTrail
        );
    }

    private SearchViewModel NoResultsViewModel(SearchParams searchParams)
    {
        var breadcrumbTrail = new BreadcrumbTrailViewModel($"We found no matches for \"{searchParams.Search}\"")
                .AddBreadcrumb("Search", "/search");

        return new SearchViewModel(
            new List<EstablishmentSearchResultsModel>(),
            null,
            searchParams.Search ?? "",
            0,
            breadcrumbTrail
        );
    }

    private IActionResult RedirectToSchoolLandingPageIfSingleResult(SearchViewModel model)
    {
        if (model.TotalCount == 1)
        {
            return RedirectToAction(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                new { area = "School", urn = model.SearchResults.FirstOrDefault()!.Urn });
        }

        return View("SearchResults", model);
    }

    private async Task<Result<SearchViewModel>> PerformSearchBasedOnUserRole(Role userRole, SearchParams searchParams,
        int pageNumber)
    {
        var (scopeType, scopeIdResult) = GetScopeInfoForRole(userRole);
        return await scopeIdResult.Then(scopeId =>
            PerformEstablishmentSearch(searchParams, scopeType, scopeId, pageNumber));
    }

    private async Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>>
        PerformSearchSuggestionsBasedOnUserRole(Role userRole, SearchParams searchParams)
    {
        var (scopeType, scopeIdResult) = GetScopeInfoForRole(userRole);
        return await scopeIdResult.Then(scopeId =>
            PerformEstablishmentSearchSuggestions(searchParams, scopeType, scopeId));
    }

    private (ScopeType, Task<Result<Optional<string>>>) GetScopeInfoForRole(Role userRole)
    {
        if (userRole.IsLaUser)
            return (ScopeType.LA, GetScopeIdForLaUser());
        if (userRole.IsMatUser)
            return (ScopeType.MAT, GetScopeIdForMatUser());
        if (userRole.IsDioceseUser)
            return (ScopeType.Diocese, Task.FromResult(User.GetDioceseName().Map(Optional<string>.Some)));
        return (ScopeType.All, Task.FromResult(Result.Success(Optional<string>.None)));
    }

    private Task<Result<Optional<string>>> GetScopeIdForLaUser()
    {
        return User.GetLocalAuthorityCode()
            .Then(laCode => _localAuthorityRepository.GetLocalAuthority(laCode)
                .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                .Map(_ => Optional<string>.Some(laCode)));
    }

    private Task<Result<Optional<string>>> GetScopeIdForMatUser()
    {
        return User.GetMatUid()
            .Then(matUid => _multiAcademyTrustRepository.GetMultiAcademyTrust(matUid)
                .MapError(error => error is NotFoundError ? Error.Unexpected(error.Message, null) : error)
                .Map(_ => Optional<string>.Some(matUid)));
    }

    private Task<Result<SearchViewModel>> PerformEstablishmentSearch(SearchParams searchParams, ScopeType scopeType,
        Optional<string> scopeId, int pageNumber)
    {
        var estabSearchRequest = new EstablishmentSearchRequest(
            searchParams.Search ?? "",
            scopeType,
            scopeId,
            Optional<int>.Some(pageNumber),
            Optional<int>.Some(Constants.SearchResultPageSize)
        );

        return _api.EstablishmentSearch(estabSearchRequest)
            .Map(DefaultViewModel)
            .DefaultIf(e => e is NotFoundError, NoResultsViewModel(searchParams));
    }

    private Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>> PerformEstablishmentSearchSuggestions(
        SearchParams searchParams, ScopeType scopeType, Optional<string> scopeId)
    {
        var request = new EstablishmentSearchSuggestionsRequest(
            searchParams.Search ?? "",
            scopeType,
            scopeId,
            Optional<int>.None
        );

        return _api.EstablishmentSearchSuggestions(request);
    }
}