using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Core;
using ASP.Core.Authorization;
using ASP.Core.Helpers;
using ASP.Core.LocalAuthorities;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Areas.Shared.EstablishmentListing;
using ASP.Web.Areas.Shared.Search;
using ASP.Web.Areas.Shared.Search.Layout.SearchPageLayout;
using ASP.Web.Areas.Shared.Search.Layout.SearchResultsPageLayout;
using ASP.Web.Areas.Shared.Search.School;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("my-schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToMySchools)]
    public class MySchoolsController : SchoolsController
    {
        private const string SearchSuggestionsUrl = $"/my-schools/suggestions/";
        
        private readonly ILocalAuthorityRepository _localAuthorityRepository;
        private readonly IMultiAcademyTrustRepository _multiAcademyTrustRepository;

        public MySchoolsController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment,
            ILocalAuthorityRepository localAuthorityRepository,
            IMultiAcademyTrustRepository multiAcademyTrustRepository
        ) : base(api, hostEnvironment)
        {
            _localAuthorityRepository = localAuthorityRepository;
            _multiAcademyTrustRepository = multiAcademyTrustRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Schools(SearchParams searchParams)
        {
            if (!Request.Query.Keys.Any(k =>
                    k.Equals(nameof(searchParams.Search), StringComparison.InvariantCultureIgnoreCase)))
            {
                return await GetSchoolsPageViewModel(searchParams.Page)
                    .DefaultIf(e => e is NotFoundError, GetEmptySchoolsPageViewModel())
                    .ToActionResult(View, _hostEnvironment);
            }

            if (!ModelState.IsValid)
            {
                return await GetSchoolsPageViewModel(searchParams.Page)
                    .DefaultIf(e => e is NotFoundError, GetEmptySchoolsPageViewModel())
                    .ToActionResult(View, _hostEnvironment);
            }

            if (string.IsNullOrEmpty(searchParams.Search))
            {
                ModelState.AddModelError(nameof(searchParams.Search), Constants.SchoolSearchTermInputValidationMessage);
                return await GetSchoolsPageViewModel(searchParams.Page)
                    .DefaultIf(e => e is NotFoundError, GetEmptySchoolsPageViewModel())
                    .ToActionResult(View, _hostEnvironment);
            }
            
            var pageNumber = PageHelper.ParsePageNumber(searchParams.Page);
            var organisationName = User.FindFirst(CustomClaimTypes.OrganisationName)?.Value;
            
            var searchUrlForNoResults = $"/my-schools/";
            
            var searchConfig = SearchConfiguration.ForSchools(  
                searchSuggestionUrl: SearchSuggestionsUrl 
            ); 
            var noResultsViewModel = new SchoolSearchResultsPageViewModel
            (
                new SearchResultsPageLayoutModel(
                    new PageViewModel(
                        GetSchoolsSearchBreadcrumbs(searchParams.Search, false),
                        $"We found no matches for \"{searchParams.Search}\""
                    ),
                    searchConfig.SearchForm,
                    searchParams.Search,
                    searchUrlForNoResults
                ),
                new List<EstablishmentListingModel>());
            
            var searchResult =
                from scopeInfo in Scope.GetScopeInfoForRole(User, _localAuthorityRepository,
                    _multiAcademyTrustRepository)
                from establishments in PerformEstablishmentSearch(
                    searchParams,
                    scopeInfo,
                    pageNumber)
                select new SchoolSearchResultsPageViewModel(
                    new SearchResultsPageLayoutModel(
                        new PageViewModel(
                            GetSchoolsSearchBreadcrumbs(searchParams.Search),
                            $"Search results for \"{searchParams.Search}\"",
                            FormatResultsSubtitle(establishments.TotalResults, organisationName)
                        ),
                        searchConfig.SearchForm,
                        searchParams.Search,
                        nameof(Schools),
                        CreatePaginationModel(establishments, nameof(Schools))
                    ),
                    MapEstablishmentListings(establishments.Results,urn => Url.Action(nameof(MySchoolsSchoolController.LandingPage),
                        "MySchoolsSchool", new { Area = "School", urn })));

            return await searchResult.DefaultIf(e => e is NotFoundError, noResultsViewModel)
                .ToActionResult(RedirectToSchoolLandingPageIfSingleResult, _hostEnvironment);
        }
        
        [HttpGet("suggestions")]
        public async Task<IActionResult> SchoolsSearchSuggestions(SearchParams searchParams)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Schools));
            }

            var searchResult =
                from scopeInfo in Scope.GetScopeInfoForRole(User, _localAuthorityRepository,
                    _multiAcademyTrustRepository)
                from searchSuggestions in PerformEstablishmentSearchSuggestions(searchParams, scopeInfo)
                select searchSuggestions; 

            return await searchResult.ToActionResult(Json, _hostEnvironment);
        }

        private BreadcrumbTrailViewModel GetSchoolsPageBreadcrumbs(string currentPage)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel(currentPage);
            return breadcrumbTrail;
        }
        
        private BreadcrumbTrailViewModel GetSchoolsSearchBreadcrumbs(
            string searchTerm,
            bool hasResults = true)
        {
            List<BreadcrumbItem> breadcrumbs =
            [
                new("My schools", "/my-schools/"),
            ];

            var currentPage = hasResults
                ? $"Search results for \"{searchTerm}\""
                : $"We found no matches for \"{searchTerm}\"";

            return new BreadcrumbTrailViewModel(breadcrumbs, currentPage);
        }
        
        private SchoolSearchPageViewModel GetEmptySchoolsPageViewModel()
        {
            var searchConfig = SearchConfiguration.ForSchools(
                searchSuggestionUrl: SearchSuggestionsUrl
            );

            return new SchoolSearchPageViewModel(
                new SearchPageLayoutModel(
                    new PageViewModel(
                        GetSchoolsPageBreadcrumbs("My schools"),
                        "My schools"
                    ),
                    searchConfig.SearchForm,
                    CreatePaginationModel(new ScopedResultsPage<EstablishmentListingDTO>(), $"/my-schools/")
                ),
                new List<EstablishmentListingModel>()
            );
        }
        
        private IActionResult RedirectToSchoolLandingPageIfSingleResult(SchoolSearchResultsPageViewModel model)
        {
            if (model.SearchResultsPage.Pagination?.TotalResults == 1)
            {
                return RedirectToAction(nameof(MySchoolsSchoolController.LandingPage), "MySchoolsSchool",
                    new { area = "School", urn = model.EstablishmentListings.FirstOrDefault()!.Urn });
            }

            return View("SchoolSearchResults", model);
        }

        
        private Task<Result<SchoolSearchPageViewModel>> GetSchoolsPageViewModel(string? page = null)
        {
            var pageNumber = PageHelper.ParsePageNumber(page);
            
            var organisationName = User.FindFirst(CustomClaimTypes.OrganisationName)?.Value;

            var searchConfig = SearchConfiguration.ForSchools(
                searchSuggestionUrl: SearchSuggestionsUrl
            );

            var result =
                from scopeInfo in Scope.GetScopeInfoForRole(User, _localAuthorityRepository,
                    _multiAcademyTrustRepository)
                from establishments in GetAllEstablishments(scopeInfo.ScopeType, scopeInfo.ScopeId, pageNumber)
                select new SchoolSearchPageViewModel(
                    new SearchPageLayoutModel(
                        new PageViewModel(
                            GetSchoolsPageBreadcrumbs("My schools"),
                            "My schools",
                            FormatResultsSubtitle(establishments.TotalResults, organisationName)
                        ),
                        searchConfig.SearchForm,
                        CreatePaginationModel(establishments, $"/my-schools/")
                    ),
                    MapEstablishmentListings(establishments.Results,urn => Url.Action(nameof(MySchoolsSchoolController.LandingPage),
                        "MySchoolsSchool", new { Area = "School", urn }))
                );

            return result;
        }
    }
}