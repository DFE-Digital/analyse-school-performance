using ASP.Application;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Core;
using ASP.Core.Helpers;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Areas.Shared.Search;
using ASP.Web.Areas.Shared.Search.School;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
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
                var result = await GetSchoolsPageViewModel(searchParams.Page);
                return View(nameof(Schools), result.GetValueOrDefault(GetEmptySchoolsPageViewModel()));
            }

            if (!ModelState.IsValid)
            {
                var result = await GetSchoolsPageViewModel(searchParams.Page);
                return View(nameof(Schools), result.GetValueOrDefault(GetEmptySchoolsPageViewModel()));
            }

            if (string.IsNullOrEmpty(searchParams.Search))
            {
                ModelState.AddModelError(nameof(searchParams.Search), Constants.SchoolSearchTermInputValidationMessage);
                var result = await GetSchoolsPageViewModel(searchParams.Page);
                return View(nameof(Schools), result.GetValueOrDefault(GetEmptySchoolsPageViewModel()));
            }

            var pageNumber = PageHelper.ParsePageNumber(searchParams.Page);

            var scopeInfo = new ScopeInfo(ScopeType.All, Optional<string>.None);

            var searchUrlForNoResults = $"/schools/";

            var noResultsVm = NoResultsViewModel(
                searchParams,
                GetSchoolsSearchNoResultsPageBreadcrumbs(searchParams.Search), 
                searchUrlForNoResults,
                SearchSuggestionsUrl, 
                "GenericSchools", 
                nameof(Schools),
                urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool", new { Area = "School", urn })
            );
				
            var schoolSearchParameters = new SchoolSearchParameters(
                nameof(Schools),
                SearchSuggestionsUrl,
				"GenericSchools",
                nameof(Schools),
                GetSchoolsSearchPageBreadcrumbs(searchParams.Search),
                urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool", new { Area = "School", urn })
            );

            var searchResult = 
                from result in PerformEstablishmentSearch(
                    searchParams, 
                    scopeInfo, 
                    pageNumber,
                    schoolSearchParameters, 
                    noResultsVm)
                select result;

            return await searchResult.ToActionResult(RedirectToSchoolLandingPageIfSingleResult, _hostEnvironment);
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> SchoolsSearchSuggestions(SearchParams searchParams)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Schools));
            }

            var searchResult = await PerformEstablishmentSearchSuggestions(searchParams);

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

        private IActionResult RedirectToSchoolLandingPageIfSingleResult(SchoolSearchViewModel model)
        {
            if (model.TotalCount == 1)
            {
                return RedirectToAction(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                    new { area = "School", urn = model.EstablishmentListingsModel.FirstOrDefault()!.Urn });
            }

            return View("SchoolSearchResults", model);
        }

        private Task<Result<SchoolsPageSearchViewModel>> GetSchoolsPageViewModel(string? page = null)
        {
            var pageNumber = PageHelper.ParsePageNumber(page);

            var result =
                from results in GetAllEstablishments(ScopeType.All, Optional<string>.None, pageNumber)
                select DefaultViewModel(
                    results,
                    new SchoolsPageSearchParameters(
                        "All schools",
                        $"{results.TotalResults:N0} schools",
                        $"/schools/",
                        GetSchoolsPageBreadcrumbs("All schools"),
                        urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                            new { Area = "School", urn },
                            "GenericSchools",
                            nameof(Schools)),
                        SearchSuggestionsUrl
                ));

            return result;
        }

        private SchoolsPageSearchViewModel GetEmptySchoolsPageViewModel()
        {
            return DefaultViewModel(
                new ScopedResultsPage<EstablishmentListingDTO>(),
                new SchoolsPageSearchParameters(
                    "All schools",
                    "0 schools",
                    $"/schools/",
                    GetSchoolsPageBreadcrumbs("All schools"),
                    urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool", new { Area = "School", urn }),
                    SearchSuggestionsUrl
                ));
        }
    }
}