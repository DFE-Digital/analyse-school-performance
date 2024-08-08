using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Core;
using ASP.Core.Establishments;
using ASP.Core.Establishments.Search;
using ASP.Core.Results;
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
                return View("Index");
            }

            // Check if the final SearchTerm is empty
            if (string.IsNullOrEmpty(searchParams.Search))
            {
                // Add error for both autocomplete and non-JS fields
                ModelState.AddModelError(nameof(searchParams.Search), Constants.SchoolSearchTermShortValidationMessage);
                return View("Index");
            }

            var estabSearchRequest = new EstablishmentSearchRequest(
                searchParams.Search,
            new Scope(ScopeType.All,
                string.Empty),
                searchParams.Page,
                Constants.SearchResultPageSize
            );

            return await _api.EstablishmentSearch(estabSearchRequest)
                .Map(DefaultViewModel)
                .DefaultIf(e => e is NotFoundError, NoResultsViewModel(searchParams))
                .ToActionResult(RedirectToSchoolLandingPageIfSingleResult, _hostEnvironment);
        }

        return View("Index");
    }

    [HttpGet("suggestions")]
    public async Task<IActionResult> SearchSuggestions(SearchParams searchParams)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        var estabSearchSuggestions = new EstablishmentSearchSuggestionsRequest(
            searchParams.Search ?? ""
        );

        return await _api.EstablishmentSearchSuggestions(estabSearchSuggestions).ToActionResult(Json, _hostEnvironment);
    }

    private SearchViewModel DefaultViewModel(SearchResultsPage<EstablishmentDetailsSearchResultDTO> result)
    {
        var breadcrumbTrail = new BreadcrumbViewModel($"Search results for \"{result.SearchTerm}\"").AddBreadcrumb("Search", "/search");

        return new SearchViewModel(
            EstablishmentSearchResultsModel.FromEstablishmentDetails(result.Results),
            new PaginationModel(
                Url.Action("Index", new { search = result.SearchTerm }) ?? "", 
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
        var breadcrumbTrail = new BreadcrumbViewModel($"We found no matches for \"{searchParams.Search}\"").AddBreadcrumb("Search", "/search");

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
            return RedirectToAction("Index", "School",
                new { area = "School", urn = model.SearchResults.FirstOrDefault()!.Urn });
        }

        return View("SearchResults", model);
    }
}