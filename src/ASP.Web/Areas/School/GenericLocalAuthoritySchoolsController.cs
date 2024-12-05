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
    [Route("local-authority/{laCode}/schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericLocalAuthoritySchoolsController : SchoolsController
    {
        private const string CurrentControllerName = "GenericLocalAuthoritySchools";
        
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
            var searchSuggestionsUrl = $"/search/suggestions/";
            var noResultsVm = NoResultsViewModel(searchParams,
                GetSchoolsSearchBreadcrumbs(searchParams.Search, laCode, laName, false),
                searchUrlForNoResults,
                searchSuggestionsUrl,
                CurrentControllerName,
                nameof(Schools),
                urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                    new { Area = "School", urn }));
            var schoolSearchParameters = new SchoolSearchParameters(nameof(Schools),
                searchSuggestionsUrl,
                CurrentControllerName,
                nameof(Schools),
                GetSchoolsSearchBreadcrumbs(searchParams.Search, laCode, laName),
                urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                    new { Area = "School", urn }),
                laName);
            var searchResult = from result in PerformEstablishmentSearch(searchParams, scopeInfo, pageNumber,
                    schoolSearchParameters, noResultsVm)
                select result;

            return await searchResult.ToActionResult(RedirectToSchoolLandingPageIfSingleResult, _hostEnvironment);
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

        private IActionResult RedirectToSchoolLandingPageIfSingleResult(SchoolSearchViewModel model)
        {
            if (model.TotalCount == 1)
            {
                return RedirectToAction(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                    new { area = "School", urn = model.EstablishmentListingsModel.FirstOrDefault()!.Urn });
            }

            return View("SchoolSearchResults", model);
        }

        private Task<Result<SchoolsPageSearchViewModel>> GetSchoolsPageViewModel(string laCode, string laName,
            string? page = null)
        {
            var pageNumber = PageHelper.ParsePageNumber(page);

            var searchSuggestionsUrl = $"/search/suggestions/";

            var result =
                from results in GetAllEstablishments(ScopeType.LA, Optional<string>.Some(laCode), pageNumber)
                select DefaultViewModel(
                    results,
                    new SchoolsPageSearchParameters(
                        "All schools",
                        $"{laName} - {results.TotalResults:N0} schools",
                        $"/local-authority/{laCode}/schools/",
                        GetSchoolsPageBreadcrumbs("All schools", laCode, laName),
                        urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                            new { Area = "School", urn }),
                        searchSuggestionsUrl));
            return result;
        }

        private SchoolsPageSearchViewModel GetEmptySchoolsPageViewModel(string laCode, string laName)
        {
            var searchSuggestionsUrl = $"/search/suggestions/";
            return DefaultViewModel(
                new ScopedResultsPage<EstablishmentListingDTO>(),
                new SchoolsPageSearchParameters(
                    "All schools",
                    "0 schools",
                    $"/local-authority/{laCode}/schools/",
                    GetSchoolsPageBreadcrumbs("All schools", laCode, laName),
                    urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool",
                        new { Area = "School", urn }),
                    searchSuggestionsUrl));
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