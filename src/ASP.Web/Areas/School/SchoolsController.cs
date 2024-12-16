using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Application.UseCases.Establishments.GetAllEstablishments;
using ASP.Core;
using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Areas.Shared.Search;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School;

public abstract class SchoolsController : Controller
{
    protected readonly IAspApiClient _api;
    protected readonly IHostEnvironment _hostEnvironment;

    protected SchoolsController(
        IAspApiClient api,
        IHostEnvironment hostEnvironment
    )
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    protected Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> GetAllEstablishments(
        ScopeType scopeType, Optional<string> scopeId, int pageNumber)
    {
        var request = new GetAllEstablishmentsRequest(
            scopeType,
            scopeId,
            Optional<int>.Some(pageNumber),
            Optional<int>.Some(Constants.SearchResultPageSize)
        );

        return _api.GetAllEstablishments(request)
            .DefaultIf(error => error is NotFoundError,
                new ScopedResultsPage<EstablishmentListingDTO>());
    }

    protected Task<Result<ScopedSearchResultsPage<EstablishmentListingDTO>>> PerformEstablishmentSearch(
        SearchParams searchParams,
        ScopeInfo scopeInfo,
        int pageNumber
    )
    {
        var searchRequest = CreateSearchRequest(searchParams, scopeInfo, pageNumber);

        var model = from searchResults in _api.EstablishmentSearch(searchRequest)
            select searchResults;

        return model;
    }
    
    protected Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>> PerformEstablishmentSearchSuggestions(
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

    protected List<EstablishmentListingModel> MapEstablishmentListings(
        IEnumerable<EstablishmentListingDTO> results,
        Func<string, string?> createSchoolUrl
    )
    {
        return EstablishmentListingModel.FromEstablishmentListingDto(results, createSchoolUrl);
    }

    protected PaginationModel CreatePaginationModel(
        ScopedSearchResultsPage<EstablishmentListingDTO> result,
        string searchUrl)
    {
        var paginationUrl = Url.Action(searchUrl, new { search = result.SearchTerm }) ?? string.Empty;

        return new PaginationModel(
            paginationUrl,
            currentPage: result.Page,
            totalResults: result.TotalResults,
            resultsPerPage: result.ResultsPerPage,
            "school",
            "schools"
        );
    }

    protected string FormatResultsSubtitle(int totalResults, string? subtitle = null)
    {
        return !string.IsNullOrEmpty(subtitle)
            ? $"{subtitle} - {totalResults:N0} schools"
            : $"{totalResults:N0} schools";
    }

    private EstablishmentSearchRequest CreateSearchRequest(
        SearchParams searchParams,
        ScopeInfo scopeInfo,
        int pageNumber)
    {
        return new EstablishmentSearchRequest(
            searchTerm: searchParams.Search ?? string.Empty,
            scopeType: scopeInfo.ScopeType,
            scopeInfo.ScopeId,
            page: Optional<int>.Some(pageNumber),
            resultsPerPage: Optional<int>.Some(Constants.SearchResultPageSize)
        );
    }

    protected PaginationModel CreatePaginationModel(
        ScopedResultsPage<EstablishmentListingDTO> result,
        string paginationUrl)
    {
        return new PaginationModel(
            paginationUrl ?? string.Empty,
            currentPage: result.Page,
            totalResults: result.TotalResults,
            resultsPerPage: result.ResultsPerPage,
            "school",
            "schools"
        );
    }
}