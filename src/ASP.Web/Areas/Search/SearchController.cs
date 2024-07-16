using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Core;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.Templating;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.Search;

[Area("Search")]
[Route("search")]
[ServiceFilter<TermsOfUseActionFilter>]
public class SearchController : Controller
{
    private readonly IAspApi _api;
    private readonly IHostEnvironment _hostEnvironment;

    public SearchController(
        IAspApi api,
        IHostEnvironment hostEnvironment
    )
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    [HttpGet("")]
    [HttpGet("index")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("search-suggestions")]
    public async Task<IActionResult> SearchSuggestions(SearchParams searchParams)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        var estabSearchSuggestions = new EstablishmentSearchSuggestionsUseCaseRequest(
            searchParams.SuggestionSearchTerm
        );

        return await _api.EstablishmentSearchSuggestions(estabSearchSuggestions).ToActionResult(Json, _hostEnvironment);
    }

    [HttpGet("search-result")]
    public async Task<IActionResult> SearchResult(SearchParams searchParams)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        if (!string.IsNullOrEmpty(searchParams.SuggestionSearchTerm))
        {
            searchParams.SearchTerm = searchParams.SuggestionSearchTerm;
        }

        if (!string.IsNullOrEmpty(searchParams.SuggestionUrn))
        {
            searchParams.SearchTerm = searchParams.SuggestionUrn;
        }

        // Check if the final SearchTerm is empty
        if (string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            // Add error for both autocomplete and non-JS fields
            ModelState.AddModelError(nameof(searchParams.SuggestionSearchTerm),
                Constants.SchoolSearchTermShortValidationMessage);
            ModelState.AddModelError(nameof(searchParams.SearchTerm), Constants.SchoolSearchTermShortValidationMessage);
            return View("Index");
        }

        var estabSearchRequest = new EstablishmentSearchUseCaseRequest(
            searchParams.SearchTerm,
            searchParams.Page,
            Constants.SearchResultPageSize
        );

        return await _api.EstablishmentSearch(estabSearchRequest)
            .Map(DefaultViewModel)
            .DefaultIf(e => e is NotFoundError, NoResultsViewModel(searchParams))
            .ToActionResult(RedirectToSchoolLandingPageIfSingleResult, _hostEnvironment);
    }

    private SearchViewModel DefaultViewModel(SearchResult<EstablishmentDetailsSearchResultDTO> result)
    {
        var breadcrumbTrail = new BreadcrumbViewModel($"Search results for \"{result.SearchTerm}\"").AddBreadcrumb("Search", "/search");

        return new SearchViewModel
        {
            SearchResults = EstablishmentSearchResultsModel.FromEstablishmentDetails(result.Results),
            PaginationModel = new PaginationModel
            {
                TotalCount = result.TotalResults,
                SearchTerm = result.SearchTerm,
                CurrentPage = result.Page,
                ResultCount = result.ResultsPerPage
            },
            SearchTerm = result.SearchTerm,
            TotalCount = result.TotalResults,
            Breadcrumbs = breadcrumbTrail
        };
    }

    private SearchViewModel NoResultsViewModel(SearchParams searchParams)
    {
        var breadcrumbTrail = new BreadcrumbViewModel($"We found no matches for \"{searchParams.SearchTerm}\"").AddBreadcrumb("Search", "/search");

        return new SearchViewModel
        {
            SearchTerm = searchParams.SearchTerm,
            TotalCount = 0,
            Breadcrumbs = breadcrumbTrail
        };
    }

    private IActionResult RedirectToSchoolLandingPageIfSingleResult(SearchViewModel model)
    {
        if (model.TotalCount == 1)
        {
            return RedirectToAction("Index", "School",
                new { area = "School", urn = model.SearchResults.FirstOrDefault()!.Urn });
        }

        return View(model);
    }
}