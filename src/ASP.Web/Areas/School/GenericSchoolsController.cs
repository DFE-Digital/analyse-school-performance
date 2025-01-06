using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Core;
using ASP.Core.Helpers;
using ASP.Core.Optionality;
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
    [Route("schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericSchoolsController : SchoolsController
    {
        private const string SearchSuggestionsUrl = $"/schools/suggestions/";

        public GenericSchoolsController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
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

            var scopeInfo = new ScopeInfo(ScopeType.All, Optional<string>.None);

            var searchUrlForNoResults = $"/schools/";

            var searchConfig = SearchConfiguration.ForSchools(
                searchSuggestionUrl: SearchSuggestionsUrl
            );

            var noResultsViewModel = new SchoolSearchResultsPageViewModel
            (
                new SearchResultsPageLayoutModel(
                    new PageViewModel(
                        GetSchoolsSearchNoResultsPageBreadcrumbs(searchParams.Search),
                        $"We found no matches for \"{searchParams.Search}\""
                    ),
                    searchConfig.SearchForm,
                    searchParams.Search,
                    searchUrlForNoResults
                ),
                new List<EstablishmentListingModel>());

            var searchResult =
                from establishments in PerformEstablishmentSearch(
                    searchParams,
                    scopeInfo,
                    pageNumber)
                select new SchoolSearchResultsPageViewModel(
                    new SearchResultsPageLayoutModel(
                        new PageViewModel(
                            GetSchoolsSearchPageBreadcrumbs(searchParams.Search),
                            $"Search results for \"{searchParams.Search}\"",
                            FormatResultsSubtitle(establishments.TotalResults)
                        ),
                        searchConfig.SearchForm,
                        searchParams.Search,
                        nameof(Schools),
                        CreatePaginationModel(establishments, nameof(Schools))
                    ),
                    MapEstablishmentListings(establishments.Results, urn =>
                        Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                            new { Area = "School", urn })));

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

            var scopeInfo = new ScopeInfo(ScopeType.All, Optional<string>.None);

            var searchResult = await PerformEstablishmentSearchSuggestions(searchParams, scopeInfo);

            return searchResult.ToActionResult(Json, _hostEnvironment);
        }

        private BreadcrumbTrailViewModel GetSchoolsPageBreadcrumbs(string currentPage)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel(currentPage);
            return breadcrumbTrail;
        }

        private BreadcrumbTrailViewModel GetSchoolsSearchPageBreadcrumbs(string searchTerm)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel($"Search results for \"{searchTerm}\"")
                .AddBreadcrumb("All schools", "/schools/");
            return breadcrumbTrail;
        }

        private BreadcrumbTrailViewModel GetSchoolsSearchNoResultsPageBreadcrumbs(string searchTerm)
        {
            var breadcrumbTrail = new BreadcrumbTrailViewModel($"We found no matches for \"{searchTerm}\"")
                .AddBreadcrumb("All schools", "/schools/");
            return breadcrumbTrail;
        }

        private IActionResult RedirectToSchoolLandingPageIfSingleResult(SchoolSearchResultsPageViewModel model)
        {
            if (model.SearchResultsPage.Pagination?.TotalResults == 1)
            {
                return RedirectToAction(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                    new { area = "School", urn = model.EstablishmentListings.FirstOrDefault()!.Urn });
            }

            return View("SchoolSearchResults", model);
        }

        private Task<Result<SchoolSearchPageViewModel>> GetSchoolsPageViewModel(string? page = null)
        {
            var pageNumber = PageHelper.ParsePageNumber(page);

            var searchConfig = SearchConfiguration.ForSchools(
                searchSuggestionUrl: SearchSuggestionsUrl
            );

            var result =
                from establishments in GetAllEstablishments(ScopeType.All, Optional<string>.None, pageNumber)
                select new SchoolSearchPageViewModel(
                    new SearchPageLayoutModel(
                        new PageViewModel(
                            GetSchoolsPageBreadcrumbs("All schools"),
                            "All schools",
                            $"{establishments.TotalResults:N0} schools"
                        ),
                        searchConfig.SearchForm,
                        CreatePaginationModel(establishments, $"/schools/")
                    ),
                    MapEstablishmentListings(establishments.Results, urn =>
                        Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                            new { Area = "School", urn }))
                );

            return result;
        }

        private SchoolSearchPageViewModel GetEmptySchoolsPageViewModel()
        {
            var searchConfig = SearchConfiguration.ForSchools(
                searchSuggestionUrl: SearchSuggestionsUrl
            );

            return new SchoolSearchPageViewModel(
                new SearchPageLayoutModel(
                    new PageViewModel(
                        GetSchoolsPageBreadcrumbs("All schools"),
                        "All schools"
                    ),
                    searchConfig.SearchForm,
                    CreatePaginationModel(new ScopedResultsPage<EstablishmentListingDTO>(), $"/schools/")
                ),
                new List<EstablishmentListingModel>()
            );
        }
    }
}