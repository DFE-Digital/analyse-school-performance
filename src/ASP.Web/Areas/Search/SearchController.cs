using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Core;
using ASP.Core.Establishments.Search;
using ASP.Core.Results;
using ASP.Core.Scope;
using ASP.Web.Areas.School;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.Search;

[Area("Search")]
[Route("search")]
[ServiceFilter<TermsOfUseActionFilter>]
public class SearchController : Controller
{
    private readonly IAspApiClient _api;
    private readonly IHostEnvironment _hostEnvironment;

    public SearchController(
        IAspApiClient api,
        IHostEnvironment hostEnvironment
    )
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(SearchParams searchParams)
    {
        if (Request.Query.Keys.Any(k => k.Equals(nameof(searchParams.Search), StringComparison.InvariantCultureIgnoreCase)))
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index));
            }
            
            // Initialize the page number to 1 by default
            var pageNumber = 1;

            // Try to parse the 'Page' property from 'searchParams' as an integer
            // If parsing is successful and the parsed value is greater than or equal to 1,
            // assign the parsed value to 'pageNumber'
            if (int.TryParse(searchParams.Page, out int intValue) && intValue >= 1)
            {
                pageNumber = intValue;
            }

            // Check if the final SearchTerm is empty
            if (string.IsNullOrEmpty(searchParams.Search))
            {
                // Add error for both autocomplete and non-JS fields
                ModelState.AddModelError(nameof(searchParams.Search), Constants.SchoolSearchTermShortValidationMessage);
                return View(nameof(Index));
            }

            var estabSearchRequest = new EstablishmentSearchRequest(
                searchParams.Search,
                ScopeType.All,
                string.Empty,
                pageNumber,
                Constants.SearchResultPageSize
            );

            return await _api.EstablishmentSearch(estabSearchRequest)
                .Map(DefaultViewModel)
                .DefaultIf(e => e is NotFoundError, NoResultsViewModel(searchParams))
                .ToActionResult(RedirectToSchoolLandingPageIfSingleResult, _hostEnvironment);
        }

        return View(nameof(Index));
    }

    [HttpGet("suggestions")]
    public async Task<IActionResult> SearchSuggestions(SearchParams searchParams)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index));
        }

        var estabSearchSuggestions = new EstablishmentSearchSuggestionsRequest(
            searchParams.Search ?? "",
            ScopeType.All,
            string.Empty
        );

        return await _api.EstablishmentSearchSuggestions(estabSearchSuggestions).ToActionResult(Json, _hostEnvironment);
    }

    private SearchViewModel DefaultViewModel(SearchResultsPage<EstablishmentListingDTO> result)
    {
        var breadcrumbTrail = new BreadcrumbTrailViewModel($"Search results for \"{result.SearchTerm}\"").AddBreadcrumb("Search", "/search");

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
        var breadcrumbTrail = new BreadcrumbTrailViewModel($"We found no matches for \"{searchParams.Search}\"").AddBreadcrumb("Search", "/search");

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
}