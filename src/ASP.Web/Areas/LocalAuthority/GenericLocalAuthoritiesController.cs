using ASP.Application;
using ASP.Application.UseCases.LocalAuthorities.DTO;
using ASP.Application.UseCases.LocalAuthorities.GetAllLocalAuthorities;
using ASP.Application.UseCases.LocalAuthorities.LocalAuthoritySearch;
using ASP.Core;
using ASP.Core.Helpers;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Web.Areas.Shared.LocalAuthorityListing;
using ASP.Web.Areas.Shared.Pagination;
using ASP.Web.Areas.Shared.Search;
using ASP.Web.Areas.Shared.Search.LocalAuthority;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
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
            var searchSuggestionsUrl = $"/search/suggestions/";
            var noResultsVm = NoResultsViewModel(searchParams,
                GetLocalAuthoritiesSearchNoResultsPageBreadcrumbs(searchParams.Search), searchUrlForNoResults,
                searchSuggestionsUrl, "GenericLocalAuthorities", nameof(LocalAuthorities));
            var localAuthoritySearchParameters = new LocalAuthoritySearchParameters(nameof(LocalAuthorities), searchSuggestionsUrl, "GenericLocalAuthorities",
                nameof(LocalAuthorities), GetLocalAuthoritiesSearchPageBreadcrumbs(searchParams.Search));
            var searchResult = from result in PerformLocalAuthoritySearch(searchParams, pageNumber,
                    localAuthoritySearchParameters, noResultsVm)
                select result;

            return await searchResult.ToActionResult(RedirectToLocalAuthorityLandingPageIfSingleResult, _hostEnvironment);
        }
        
        private LocalAuthoritiesPageSearchViewModel DefaultViewModel(ResultsPage<LocalAuthorityDTO> result,
            LocalAuthoritiesPageSearchParameters searchParameters)
        {
            var paginationModel = CreatePaginationModel(result, searchParameters.PaginationUrl);
            var localAuthoritiesListings = MapLocalAuthoritiesListings(result.Results);
            
            return new LocalAuthoritiesPageSearchViewModel(
                title: searchParameters.Title,
                subTitle: searchParameters.SubTitle,
                totalCount: result.TotalResults,
                paginationModel: paginationModel,
                breadcrumbs: searchParameters.Breadcrumbs!,
                localAuthoritiesListingModel: localAuthoritiesListings,
                controller: searchParameters.Controller,
                controllerAction: searchParameters.ControllerAction,
                searchSuggestionUrl: searchParameters.SearchSuggestionUrl,
                inputLabel: searchParameters.InputLabel,
                inputValidationMessage: searchParameters.InputValidationMessage
            );
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

        private IActionResult RedirectToLocalAuthorityLandingPageIfSingleResult(LocalAuthoritySearchViewModel model)
        {
            if (model.TotalCount == 1)
            {
                return RedirectToAction(nameof(GenericLocalAuthorityController.LandingPage), "GenericLocalAuthority",
                    new
                    {
                        area = "LocalAuthority",
                        laCode = model.LocalAuthoritiesListingModel.FirstOrDefault()!.Code
                    });
            }

            return View("LocalAuthoritySearchResults", model);
        }
        
        private Task<Result<LocalAuthoritySearchViewModel>> PerformLocalAuthoritySearch(
            SearchParams searchParams,
            int pageNumber,
            LocalAuthoritySearchParameters parameters,
            LocalAuthoritySearchViewModel noResultsViewModel)
        {
            var searchRequest = CreateSearchRequest(searchParams, pageNumber);

            var model = from searchResults in _api.LocalAuthoritySearch(searchRequest)
                select GetLocalAuthoritySearchViewModel(searchResults, parameters, searchParams.Search);

            return model.DefaultIf(e => e is NotFoundError, noResultsViewModel);
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
        
        private LocalAuthoritySearchViewModel GetLocalAuthoritySearchViewModel(
            SearchResultsPage<LocalAuthorityDTO> result,
            LocalAuthoritySearchParameters parameters,
            string? searchTerm)
        {
            return new LocalAuthoritySearchViewModel(
                MapLocalAuthoritiesListings(result.Results),
                paginationModel: CreatePaginationModel(result, parameters.SearchUrl, searchTerm),
                searchTerm: result.SearchTerm,
                result.TotalResults,
                breadcrumbs: parameters.Breadcrumbs!,
                parameters.SearchUrl,
                searchSuggestionUrl: parameters.SearchSuggestionUrl,
                controller: parameters.Controller,
                controllerAction: parameters.ControllerAction,
                inputLabel: parameters.InputLabel,
                inputValidationMessage: parameters.InputValidationMessage,
                FormatResultsTitle(result.TotalResults)
            );
        }

        private Task<Result<LocalAuthoritiesPageSearchViewModel>> GetLocalAuthoritiesPageViewModel(SearchParams searchParams)
        {
            var pageNumber = PageHelper.ParsePageNumber(searchParams.Page);

            var searchSuggestionsUrl = $"/search/suggestions/";
            
            var result =
                from results in GetAllLocalAuthorities(pageNumber)
                select DefaultViewModel(
                    results,
                    new LocalAuthoritiesPageSearchParameters("All local authorities", $"{results.TotalResults:N0} local authorities",
                        $"/local-authorities/", GetLocalAuthoritiesPageBreadcrumbs("All local authorities"),
                        "GenericLocalAuthorities", nameof(LocalAuthorities), searchSuggestionsUrl));
            return result;
        }

        private LocalAuthoritiesPageSearchViewModel GetEmptyLocalAuthoritiesPageViewModel()
        {
            var searchSuggestionsUrl = $"/search/suggestions/";
            return DefaultViewModel(
                new ResultsPage<LocalAuthorityDTO>(),
                new LocalAuthoritiesPageSearchParameters("All local authorities", "0 local authorities",
                    $"/local-authorities/", GetLocalAuthoritiesPageBreadcrumbs("All local authorities"),
                    "GenericLocalAuthorities", nameof(LocalAuthorities), searchSuggestionsUrl));
        }
        
        protected LocalAuthoritySearchViewModel NoResultsViewModel(SearchParams searchParams,
            BreadcrumbTrailViewModel breadcrumbTrail,
            string searchUrl, string searchSuggestionUrl,
            string controller, string controllerAction)
        {
            return new LocalAuthoritySearchViewModel(
                new List<LocalAuthoritiesListingModel>(),
                null,
                searchParams.Search ?? "",
                0,
                breadcrumbTrail,
                searchUrl,
                searchSuggestionUrl,
                controller,
                controllerAction,
                Constants.LaSearchFormSearchTermInputLabel,
                Constants.LaSearchTermInputValidationMessage
            );
        }
        
        private PaginationModel CreatePaginationModel(
            ResultsPage<LocalAuthorityDTO> result, 
            string searchUrl, string? searchTerm)
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
        
        private string FormatResultsTitle(int totalResults)
        {
            return $"{totalResults:N0} local authorities";
        }
    }
}
