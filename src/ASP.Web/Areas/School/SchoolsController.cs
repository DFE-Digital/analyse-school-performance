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
using ASP.Web.Areas.Shared.Search.School;
using ASP.Web.Core.BreadcrumbTrail;
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

    protected SchoolsPageSearchViewModel DefaultViewModel(ScopedResultsPage<EstablishmentListingDTO> result,
        SchoolsPageSearchParameters searchParameters)
    {
        var paginationModel = CreatePaginationModel(result, searchParameters.PaginationUrl);
        var establishmentListings = MapEstablishmentListings(result.Results, searchParameters.CreateSchoolUrl);

        return new SchoolsPageSearchViewModel(
            title: searchParameters.Title,
            subTitle: searchParameters.SubTitle,
            totalCount: result.TotalResults,
            paginationModel: paginationModel,
            breadcrumbTrail: searchParameters.BreadcrumbTrail!,
            establishmentListingsModel: establishmentListings,
            searchSuggestionUrl: searchParameters.SearchSuggestionUrl
        );
    }

    protected Task<Result<SchoolSearchViewModel>> PerformEstablishmentSearch(
        SearchParams searchParams,
        ScopeInfo scopeInfo,
        int pageNumber,
        SchoolSearchParameters parameters,
        SchoolSearchViewModel noResultsViewModel
    )
    {
        var searchRequest = CreateSearchRequest(searchParams, scopeInfo, pageNumber);

        var model = from searchResults in _api.EstablishmentSearch(searchRequest)
                    select GetSchoolSearchViewModel(searchResults, parameters);

        return model.DefaultIf(e => e is NotFoundError, noResultsViewModel);
    }

    protected Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>> PerformEstablishmentSearchSuggestions(
        SearchParams searchParams)
    {
        var scopeInfo = new ScopeInfo(ScopeType.All, Optional<string>.None);

        var request = new EstablishmentSearchSuggestionsRequest(
            searchParams.Search ?? "",
            scopeInfo.ScopeType,
            scopeInfo.ScopeId,
            Optional<int>.None
        );

        return _api.EstablishmentSearchSuggestions(request);
    }

    protected SchoolSearchViewModel NoResultsViewModel(
        SearchParams searchParams,
        BreadcrumbTrailViewModel breadcrumbTrail,
        string searchUrl, 
        string searchSuggestionUrl,
        string controller, 
        string controllerAction, 
        Func<string, string?> createSchoolUrl
    )
    {
        return new SchoolSearchViewModel(
            establishmentListingsModel: new List<EstablishmentListingModel>(),
            paginationModel: null,
            searchTerm: searchParams.Search ?? "",
            totalCount: 0,
            breadcrumbTrail: breadcrumbTrail,
            controller: controller,
            controllerAction: controllerAction,
            searchUrl: searchUrl,
            searchSuggestionUrl: searchSuggestionUrl,
            createSchoolUrl
        );
    }

    private List<EstablishmentListingModel> MapEstablishmentListings(
        IEnumerable<EstablishmentListingDTO> results,
        Func<string, string?> createSchoolUrl
    )
    {
        return EstablishmentListingModel.FromEstablishmentListingDto(results, createSchoolUrl);
    }

    private SchoolSearchViewModel GetSchoolSearchViewModel(
        ScopedSearchResultsPage<EstablishmentListingDTO> result,
        SchoolSearchParameters parameters
    )
    {
        return new SchoolSearchViewModel(
            MapEstablishmentListings(result.Results, parameters.CreateSchoolUrl),
            paginationModel: CreatePaginationModel(result, parameters.SearchUrl),
            searchTerm: result.SearchTerm,
            result.TotalResults,
            breadcrumbTrail: parameters.BreadcrumbTrail!,
            controller: parameters.Controller!,
            controllerAction: parameters.ControllerAction!,
            parameters.SearchUrl,
            searchSuggestionUrl: parameters.SearchSuggestionUrl,
            createSchoolUrl: parameters.CreateSchoolUrl,
            FormatResultsTitle(result.TotalResults)
        );
    }

    private PaginationModel CreatePaginationModel(
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

    private string FormatResultsTitle(int totalResults)
    {
        return $"{totalResults:N0} schools";
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

    private PaginationModel CreatePaginationModel(
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