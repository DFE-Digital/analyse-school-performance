using ASP.Application;
using ASP.Core.Helpers;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Core.Scoping;
using ASP.Web.Areas.School;
using ASP.Web.Areas.Shared.Search.School;
using ASP.Web.Core.BreadcrumbTrail;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.LocalAuthority
{
    [Area("LocalAuthority")]
    [Route("local-authority/{laCode}/schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericSchoolsLocalAuthorityController : SchoolsController
    {
        public GenericSchoolsLocalAuthorityController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment
        ) : base(api, hostEnvironment)
        {
        }

        [HttpGet("")]
        public Task<IActionResult> Schools(string laCode, string? page)
        {
            var pageNumber = PageHelper.ParsePageNumber(page);
            var searchSuggestionsUrl = $"/search/suggestions/";

            var result =
                from results in GetAllEstablishments(ScopeType.LA, Optional<string>.Some(laCode), pageNumber)
                from laName in GetLocalAuthorityName(laCode)
                select DefaultViewModel(
                    results,
                    new SchoolsPageParameters("All schools", $"{laName} - {results.TotalResults:N0} schools",
                        $"/local-authority/{laCode}/schools/", GetSchoolsPageBreadcrumbs("All schools", laCode, laName),
                        "GenericSchoolsLocalAuthority", nameof(Schools), searchSuggestionsUrl)
                );

            return result.ToActionResult(View, _hostEnvironment);
        }

        private BreadcrumbTrailViewModel GetSchoolsPageBreadcrumbs(string currentPage, string laCode, string laName)
        {
            List<BreadcrumbItem> breadcrumbs =
            [
                new("All local authorities", $"/local-authorities"),
                new(laName, $"/local-authority/{laCode}")
            ];
            return new BreadcrumbTrailViewModel(breadcrumbs, currentPage);
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