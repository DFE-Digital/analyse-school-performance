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
    [Route("local-authority/{laCode}/schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericLocalAuthoritySchoolsController : SchoolsController
    {
        public GenericLocalAuthoritySchoolsController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public async Task<IActionResult> Schools(string laCode, SearchParams searchParams)
        {
            var laName = await GetLocalAuthorityName(laCode).GetValueOrDefault("Missing local authority name");

            if (!Request.Query.Keys.Any(k =>
                    k.Equals(nameof(searchParams.Search), StringComparison.InvariantCultureIgnoreCase)))
            {
                var result = await GetSchoolsPageViewModel(laCode, laName, searchParams.Page);
                return View(nameof(Schools), result.GetValueOrDefault(GetEmptySchoolsPageViewModel(laCode, laName)));
            }

            if (!ModelState.IsValid)
            {
                var result = await GetSchoolsPageViewModel(laCode, laName, searchParams.Page);
                return View(nameof(Schools), result.GetValueOrDefault(GetEmptySchoolsPageViewModel(laCode, laName)));
            }

            if (string.IsNullOrEmpty(searchParams.Search))
            {
                ModelState.AddModelError(nameof(searchParams.Search), Constants.SchoolSearchTermInputValidationMessage);
                var result = await GetSchoolsPageViewModel(laCode, laName, searchParams.Page);
                return View(nameof(Schools), result.GetValueOrDefault(GetEmptySchoolsPageViewModel(laCode, laName)));
            }

            var pageNumber = PageHelper.ParsePageNumber(searchParams.Page);

            var scopeInfo = new ScopeInfo(ScopeType.LA, Optional<string>.Some(laCode));

            var searchUrlForNoResults = $"/local-authority/{laCode}/schools/";
            var searchSuggestionsUrl = $"/local-authority/{laCode}/schools/suggestions";

            var searchConfig = SearchConfiguration.ForSchools(
                searchSuggestionUrl: searchSuggestionsUrl
            );
            var noResultsViewModel = new SchoolSearchResultsPageViewModel
            (
                new SearchResultsPageLayoutModel(
                    new PageViewModel(
                        GetSchoolsSearchBreadcrumbs(searchParams.Search, laCode, laName, false),
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
                            GetSchoolsSearchBreadcrumbs(searchParams.Search, laCode, laName),
                            $"Search results for \"{searchParams.Search}\"",
                            FormatResultsSubtitle(establishments.TotalResults, laName)
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
        public async Task<IActionResult> GenericLocalAuthoritySchoolsSearchSuggestions(string laCode,
            SearchParams searchParams)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Schools));
            }

            var scopeInfo = new ScopeInfo(ScopeType.LA, Optional<string>.Some(laCode));

            var searchResult = await PerformEstablishmentSearchSuggestions(searchParams, scopeInfo);

            return searchResult.ToActionResult(Json, _hostEnvironment);
        }

        private BreadcrumbTrailViewModel GetSchoolsPageBreadcrumbs(string currentPage, string laCode, string laName)
        {
            List<BreadcrumbItem> breadcrumbs =
            [
                new("All local authorities", $"/local-authorities/"),
                new(laName, $"/local-authority/{laCode}/")
            ];
            return new BreadcrumbTrailViewModel(breadcrumbs, currentPage);
        }

        private BreadcrumbTrailViewModel GetSchoolsSearchBreadcrumbs(
            string searchTerm,
            string laCode,
            string laName,
            bool hasResults = true)
        {
            List<BreadcrumbItem> breadcrumbs =
            [
                new("All local authorities", "/local-authorities/"),
                new(laName, $"/local-authority/{laCode}/"),
                new("All schools", $"/local-authority/{laCode}/schools/")
            ];

            var currentPage = hasResults
                ? $"Search results for \"{searchTerm}\""
                : $"We found no matches for \"{searchTerm}\"";

            return new BreadcrumbTrailViewModel(breadcrumbs, currentPage);
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

        private Task<Result<SchoolSearchPageViewModel>> GetSchoolsPageViewModel(string laCode, string laName,
            string? page = null)
        {
            var pageNumber = PageHelper.ParsePageNumber(page);
            var searchSuggestionsUrl = $"/local-authority/{laCode}/schools/suggestions";

            var searchConfig = SearchConfiguration.ForSchools(
                searchSuggestionUrl: searchSuggestionsUrl
            );

            var result =
                from establishments in GetAllEstablishments(ScopeType.LA, Optional<string>.Some(laCode), pageNumber)
                select new SchoolSearchPageViewModel(
                    new SearchPageLayoutModel(
                        new PageViewModel(
                            GetSchoolsPageBreadcrumbs("All schools", laCode, laName),
                            "All schools",
                            $"{laName} - {establishments.TotalResults:N0} schools"
                        ),
                        searchConfig.SearchForm,
                        CreatePaginationModel(establishments, $"/local-authority/{laCode}/schools/")
                    ),
                    MapEstablishmentListings(establishments.Results, urn =>
                        Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                            new { Area = "School", urn }))
                );
            return result;
        }

        private SchoolSearchPageViewModel GetEmptySchoolsPageViewModel(string laCode, string laName)
        {
            var searchSuggestionsUrl = $"/local-authority/{laCode}/schools/suggestions";
            var searchConfig = SearchConfiguration.ForSchools(
                searchSuggestionUrl: searchSuggestionsUrl
            );

            return new SchoolSearchPageViewModel(
                new SearchPageLayoutModel(
                    new PageViewModel(
                        GetSchoolsPageBreadcrumbs("All schools", laCode, laName),
                        "All schools"
                    ),
                    searchConfig.SearchForm,
                    CreatePaginationModel(new ScopedResultsPage<EstablishmentListingDTO>(),
                        $"/local-authority/{laCode}/schools/")
                ),
                new List<EstablishmentListingModel>()
            );
        }

        private Task<Result<string>> GetLocalAuthorityName(string laCode)
        {
            return
                from la in _api.GetLocalAuthority(new(laCode))
                select string.IsNullOrWhiteSpace(la.Name)
                    ? "Missing local authority name"
                    : la.Name;
        }
    }
}