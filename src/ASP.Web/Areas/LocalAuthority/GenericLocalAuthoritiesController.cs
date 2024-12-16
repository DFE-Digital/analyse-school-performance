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
using ASP.Web.Areas.Shared.LocalAuthorityListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Areas.Shared.Search;
using ASP.Web.Areas.Shared.Search.Layout.SearchPageLayout;
using ASP.Web.Areas.Shared.Search.Layout.SearchResultsPageLayout;
using ASP.Web.Areas.Shared.Search.LocalAuthority;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
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
        public async Task<IActionResult> LocalAuthorities(SearchParams searchParams)
        {
            if (!Request.Query.Keys.Any(k =>
                    k.Equals(nameof(searchParams.Search), StringComparison.InvariantCultureIgnoreCase)))
            {
                var result = await GetLocalAuthoritiesPageViewModel(searchParams);
                return View(nameof(LocalAuthorities), result.GetValueOrDefault(GetEmptyLocalAuthoritiesPageViewModel()));
            }

            if (!ModelState.IsValid)
            {
                var result = await GetLocalAuthoritiesPageViewModel(searchParams);
                return View(nameof(LocalAuthorities), result.GetValueOrDefault(GetEmptyLocalAuthoritiesPageViewModel()));
            }

            if (string.IsNullOrEmpty(searchParams.Search))
            {
                ModelState.AddModelError(nameof(searchParams.Search), Constants.LaSearchTermInputValidationMessage);
                var result = await GetLocalAuthoritiesPageViewModel(searchParams);
                return View(nameof(LocalAuthorities), result.GetValueOrDefault(GetEmptyLocalAuthoritiesPageViewModel()));
            }
            
            var pageNumber = PageHelper.ParsePageNumber(searchParams.Page);
            
            var searchUrlForNoResults = $"/local-authorities/";
            
            var searchConfig = SearchConfiguration.ForLocalAuthorities(  
                searchSuggestionUrl: SearchSuggestionsUrl 
            );
            
            var noResultsViewModel = new LocalAuthoritySearchResultsPageViewModel
            (
                new SearchResultsPageLayoutModel(
                    new PageViewModel(
                        GetLocalAuthoritiesSearchNoResultsPageBreadcrumbs(searchParams.Search),
                        $"We found no matches for \"{searchParams.Search}\""
                    ),
                    searchConfig.SearchForm,
                    searchParams.Search,
                    searchUrlForNoResults
                ),
                new List<LocalAuthoritiesListingModel>());
            
            var searchResult =
                from localAuthorities in PerformLocalAuthoritySearch(
                    searchParams,
                    pageNumber)
                select new LocalAuthoritySearchResultsPageViewModel(
                    new SearchResultsPageLayoutModel(
                        new PageViewModel(
                            GetLocalAuthoritiesSearchPageBreadcrumbs(searchParams.Search),
                            $"Search results for \"{searchParams.Search}\"",
                            FormatResultsSubtitle(localAuthorities.TotalResults)
                        ),
                        searchConfig.SearchForm,
                        searchParams.Search,
                        nameof(LocalAuthorities),
                        CreatePaginationModel(localAuthorities, nameof(LocalAuthorities), searchParams.Search)
                    ),
                    MapLocalAuthoritiesListings(localAuthorities.Results));

            return await searchResult.DefaultIf(e => e is NotFoundError, noResultsViewModel)
                .ToActionResult(RedirectToLocalAuthorityLandingPageIfSingleResult, _hostEnvironment);
        }
        
        [HttpGet("suggestions")]
        public async Task<IActionResult> LocalAuthoritySearchSuggestions(SearchParams searchParams)
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
                .MapError(e => e is NotFoundError ? Error.Unexpected(e.Message, null): e);
        }
        
        private BreadcrumbTrailViewModel GetLocalAuthoritiesPageBreadcrumbs(string currentPage)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel(currentPage);
            return breadcrumbTrail;
        }
        
        private BreadcrumbTrailViewModel GetLocalAuthoritiesSearchPageBreadcrumbs(string searchTerm)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel($"Search results for \"{searchTerm}\"")
                .AddBreadcrumb("All local authorities", "/local-authorities/");
            return breadcrumbTrail;
        }
        
        private BreadcrumbTrailViewModel GetLocalAuthoritiesSearchNoResultsPageBreadcrumbs(string searchTerm)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel($"We found no matches for \"{searchTerm}\"")
                .AddBreadcrumb("All local authorities", "/local-authorities/");
            return breadcrumbTrail;
        }

        private IActionResult RedirectToLocalAuthorityLandingPageIfSingleResult(LocalAuthoritySearchResultsPageViewModel model)
        {
            if (model.SearchResultsPage.Pagination?.TotalResults == 1)
            {
                return RedirectToAction(nameof(GenericLocalAuthorityController.LandingPage), "GenericLocalAuthority",
                    new
                    {
                        area = "LocalAuthority",
                        laCode = model.LocalAuthorityListings.FirstOrDefault()!.Code
                    });
            }

            return View("LocalAuthoritySearchResults", model);
        }
        
        private Task<Result<SearchResultsPage<LocalAuthorityDTO>>> PerformLocalAuthoritySearch(
            SearchParams searchParams,
            int pageNumber)
        {
            var searchRequest = CreateSearchRequest(searchParams, pageNumber);

            var model = from searchResults in _api.LocalAuthoritySearch(searchRequest)
                    select searchResults;

            return model;
        }
        
        private LocalAuthoritySearchRequest CreateSearchRequest(
            SearchParams searchParams,
            int pageNumber)
        {
            return new LocalAuthoritySearchRequest(
                searchTerm: searchParams.Search ?? string.Empty,
                page: Optional<int>.Some(pageNumber),
                resultsPerPage: Optional<int>.Some(Constants.SearchResultPageSize)
            );
        }

        private Task<Result<LocalAuthoritySearchPageViewModel>> GetLocalAuthoritiesPageViewModel(SearchParams searchParams)
        {
            var pageNumber = PageHelper.ParsePageNumber(searchParams.Page);
            
            var searchConfig = SearchConfiguration.ForLocalAuthorities(  
                searchSuggestionUrl: SearchSuggestionsUrl 
            ); 
            
            var result =
                from localAuthorities in GetAllLocalAuthorities(pageNumber)
                select new LocalAuthoritySearchPageViewModel(
                    new SearchPageLayoutModel(
                        new PageViewModel(
                            GetLocalAuthoritiesPageBreadcrumbs("All local authorities"),
                            "All local authorities",
                            $"{localAuthorities.TotalResults:N0} local authorities"
                        ),
                        searchConfig.SearchForm,
                        CreatePaginationModel(localAuthorities,$"/local-authorities/")
                    ),
                    MapLocalAuthoritiesListings(localAuthorities.Results)
                );

            return result;
        }

        private LocalAuthoritySearchPageViewModel GetEmptyLocalAuthoritiesPageViewModel()
        {
            var searchConfig = SearchConfiguration.ForLocalAuthorities(  
                searchSuggestionUrl: SearchSuggestionsUrl 
            ); 
            
            return new LocalAuthoritySearchPageViewModel(
                new SearchPageLayoutModel(
                    new PageViewModel(
                        GetLocalAuthoritiesPageBreadcrumbs("All local authorities"),
                        "All local authorities"
                    ),
                    searchConfig.SearchForm,
                    CreatePaginationModel(new ScopedResultsPage<LocalAuthorityDTO>(),$"/local-authorities/")
                ),
                new List<LocalAuthoritiesListingModel>()
            );
        }
        
        private PaginationModel CreatePaginationModel(
            ResultsPage<LocalAuthorityDTO> result, 
            string searchUrl, string searchTerm)
        {
            var paginationUrl = Url.Action(searchUrl, new { search = searchTerm }) ?? string.Empty;
            return new PaginationModel(
                paginationUrl ?? string.Empty,
                currentPage: result.Page,
                totalResults: result.TotalResults,
                resultsPerPage: result.ResultsPerPage,
                "local authority",
                "local authorities"
            );
        }
        
        private PaginationModel CreatePaginationModel(
            ResultsPage<LocalAuthorityDTO> result, 
            string paginationUrl)
        {
            return new PaginationModel(
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
            SearchParams searchParams)
        {
            
            var request = new LocalAuthoritySearchSuggestionsRequest(
                searchParams.Search ?? "",
                Optional<int>.None
            );

            return _api.LocalAuthoritySearchSuggestions(request);
        }
    }
}
