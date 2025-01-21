using ASP.Application;
using ASP.Application.UseCases.LocalAuthorities.DTO;
using ASP.Application.UseCases.LocalAuthorities.GetAllLocalAuthorities;
using ASP.Application.UseCases.LocalAuthorities.LocalAuthoritySearch;
using ASP.Application.UseCases.LocalAuthorities.LocalAuthoritySearchSuggestions;
using ASP.Core;
using ASP.Core.Helpers;
using ASP.Core.LocalAuthorities.LocalAuthoritySearchSuggestions;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Core.Utilities;
using ASP.Web.Features.Search;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
using ASP.Web.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    [Area("LocalAuthority")]
    [Route("local-authorities")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericLocalAuthoritiesController : Controller
    {
        private const string SearchSuggestionsUrl = $"/local-authorities/suggestions/";
        private readonly IAspApiClient _api;
        private readonly IHostEnvironment _hostEnvironment;

        public GenericLocalAuthoritiesController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        )
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        [HttpGet("")]
        public async Task<IActionResult> LocalAuthorities(SearchParameters searchParams)
        {
            if (!Request.Query.Keys.Any(k =>
                    k.Equals(nameof(searchParams.Search), StringComparison.InvariantCultureIgnoreCase)))
            {
                var result = await GetLocalAuthoritiesPageViewModel(searchParams);
                return View(result.GetValueOrDefault(GetEmptyLocalAuthoritiesPageViewModel()));
            }

            if (!ModelState.IsValid)
            {
                var result = await GetLocalAuthoritiesPageViewModel(searchParams);
                return View(result.GetValueOrDefault(GetEmptyLocalAuthoritiesPageViewModel()));
            }

            if (string.IsNullOrEmpty(searchParams.Search))
            {
                ModelState.AddModelError(nameof(searchParams.Search), Constants.LaSearchTermInputValidationMessage);
                var result = await GetLocalAuthoritiesPageViewModel(searchParams);
                return View(result.GetValueOrDefault(GetEmptyLocalAuthoritiesPageViewModel()));
            }

            var pageNumber = PageHelper.ParsePageNumber(searchParams.Page);

            var searchUrlForNoResults = $"/local-authorities/";

            var noResultsViewModel = new LocalAuthoritySearchPageViewModel
            (
                new PageViewModel(
                    GetLocalAuthoritiesSearchPageBreadcrumbs(),
                    $"We found no matches for \"{searchParams.Search}\""
                ),
                new SearchResultsNotFoundViewModel(searchParams.Search, searchUrlForNoResults),
                new List<LocalAuthoritiesListingModel>());

            var searchResult =
                from localAuthorities in PerformLocalAuthoritySearch(
                    searchParams,
                    pageNumber)
                select new LocalAuthoritySearchPageViewModel(
                    new PageViewModel(
                        GetLocalAuthoritiesSearchPageBreadcrumbs(),
                        $"Search results for \"{searchParams.Search}\"",
                        FormatResultsSubtitle(localAuthorities.TotalResults)
                    ),
                    SearchFormViewModel.ForLocalAuthorities(
                        searchParams.Search,
                        SearchSuggestionsUrl,
                        CreatePaginationModel(localAuthorities, nameof(LocalAuthorities), searchParams.Search)
                    ),
                    MapLocalAuthoritiesListings(localAuthorities.Results));

            return await searchResult.DefaultIf(e => e is NotFoundError, noResultsViewModel)
                .ToActionResult(RedirectToLocalAuthorityLandingPageIfSingleResult, _hostEnvironment);
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> LocalAuthoritySearchSuggestions(SearchParameters searchParams)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(LocalAuthorities));
            }

            var searchResult = await PerformLocalAuthoritySearchSuggestions(searchParams);

            return searchResult.ToActionResult(Json, _hostEnvironment);
        }

        private Task<Result<ResultsPage<LocalAuthorityDTO>>> GetAllLocalAuthorities(int pageNumber)
        {
            var request = new GetAllLocalAuthoritiesRequest(
                Optional<int>.Some(pageNumber),
                Optional<int>.Some(Constants.SearchResultPageSize)
            );

            return _api.GetAllLocalAuthorities(request)
                .MapError(e => e is NotFoundError ? Error.Unexpected(e.Message, null) : e);
        }

        private BreadcrumbTrailViewModel GetLocalAuthoritiesPageBreadcrumbs()
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel();
            return breadcrumbTrail;
        }

        private BreadcrumbTrailViewModel GetLocalAuthoritiesSearchPageBreadcrumbs()
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel()
                .AddBreadcrumb("All local authorities", "/local-authorities/");
            return breadcrumbTrail;
        }

        private IActionResult RedirectToLocalAuthorityLandingPageIfSingleResult(LocalAuthoritySearchPageViewModel model)
        {
            if (model.Search is SearchFormViewModel searchForm && searchForm.Pagination?.TotalResults == 1)
            {
                return RedirectToAction(nameof(GenericLocalAuthorityController.LandingPage), "GenericLocalAuthority",
                    new
                    {
                        area = "LocalAuthority",
                        laCode = model.LocalAuthorityListings.FirstOrDefault()!.Code
                    });
            }

            return View(nameof(LocalAuthorities), model);
        }

        private Task<Result<SearchResultsPage<LocalAuthorityDTO>>> PerformLocalAuthoritySearch(
            SearchParameters searchParams,
            int pageNumber)
        {
            var searchRequest = CreateSearchRequest(searchParams, pageNumber);

            var model = from searchResults in _api.LocalAuthoritySearch(searchRequest)
                        select searchResults;

            return model;
        }

        private LocalAuthoritySearchRequest CreateSearchRequest(
            SearchParameters searchParams,
            int pageNumber)
        {
            return new LocalAuthoritySearchRequest(
                searchTerm: searchParams.Search ?? string.Empty,
                page: Optional<int>.Some(pageNumber),
                resultsPerPage: Optional<int>.Some(Constants.SearchResultPageSize)
            );
        }

        private Task<Result<LocalAuthoritySearchPageViewModel>> GetLocalAuthoritiesPageViewModel(SearchParameters searchParams)
        {
            var pageNumber = PageHelper.ParsePageNumber(searchParams.Page);

            var result =
                from localAuthorities in GetAllLocalAuthorities(pageNumber)
                select new LocalAuthoritySearchPageViewModel(
                    new PageViewModel(
                        GetLocalAuthoritiesPageBreadcrumbs(),
                        "All local authorities",
                        $"{localAuthorities.TotalResults:N0} local authorities"
                    ),
                    SearchFormViewModel.ForLocalAuthorities(
                        searchParams.Search ?? string.Empty,
                        SearchSuggestionsUrl,
                        CreatePaginationModel(localAuthorities, $"/local-authorities/")
                    ),
                    MapLocalAuthoritiesListings(localAuthorities.Results)
                );

            return result;
        }

        private LocalAuthoritySearchPageViewModel GetEmptyLocalAuthoritiesPageViewModel()
        {
            return new LocalAuthoritySearchPageViewModel(
                new PageViewModel(
                    GetLocalAuthoritiesPageBreadcrumbs(),
                    "All local authorities"
                ),
                SearchFormViewModel.ForLocalAuthorities(
                    string.Empty,
                    SearchSuggestionsUrl,
                    CreatePaginationModel(new ScopedResultsPage<LocalAuthorityDTO>(), $"/local-authorities/")
                ),
                new List<LocalAuthoritiesListingModel>()
            );
        }

        private PaginationViewModel CreatePaginationModel(
            ResultsPage<LocalAuthorityDTO> result,
            string searchUrl, string searchTerm)
        {
            var paginationUrl = Url.Action(searchUrl, new { search = searchTerm }) ?? string.Empty;
            return new PaginationViewModel(
                paginationUrl ?? string.Empty,
                currentPage: result.Page,
                totalResults: result.TotalResults,
                resultsPerPage: result.ResultsPerPage,
                "local authority",
                "local authorities"
            );
        }

        private PaginationViewModel CreatePaginationModel(
            ResultsPage<LocalAuthorityDTO> result,
            string paginationUrl)
        {
            return new PaginationViewModel(
                paginationUrl ?? string.Empty,
                currentPage: result.Page,
                totalResults: result.TotalResults,
                resultsPerPage: result.ResultsPerPage,
                "local authority",
                "local authorities"
            );
        }

        private List<LocalAuthoritiesListingModel> MapLocalAuthoritiesListings(
            IEnumerable<LocalAuthorityDTO> results)
        {
            return LocalAuthoritiesListingModel.FromLocalAuthoritiesListingDto(
                results,
                laCode => Url.Action(nameof(GenericLocalAuthorityController.LandingPage), "GenericLocalAuthority", new { Area = "LocalAuthority", laCode })
            );
        }

        private string FormatResultsSubtitle(int totalResults)
        {
            return $"{totalResults:N0} local authorities";
        }

        private Task<Result<LocalAuthoritySearchSuggestionsResult<LocalAuthorityDTO>>> PerformLocalAuthoritySearchSuggestions(
            SearchParameters searchParams)
        {

            var request = new LocalAuthoritySearchSuggestionsRequest(
                searchParams.Search ?? "",
                Optional<int>.None
            );

            return _api.LocalAuthoritySearchSuggestions(request);
        }
    }
}
