using ASP.Application;
using ASP.Core.Optionality;
using ASP.Core.Results;
using ASP.Domain.Establishments;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.Search;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("local-authority/{laCode:int:length(3)}/schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericLocalAuthoritySchoolsController : SchoolsController
    {
        private readonly SchoolSearchController _schoolSearchController;

        public GenericLocalAuthoritySchoolsController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment,
            IOptions<SearchOptions> searchOptions
        ) : base(api, hostEnvironment)
        {
            _schoolSearchController = new SchoolSearchController(nameof(Schools), "GenericLocalAuthoritySchools", ["laCode"], _api,
                searchOptions.Value, makeSchoolUrl: urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool", new { urn }));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            _schoolSearchController.BindContext(context);
        }

        [HttpGet($"{SchoolSearchController.SubRouteTemplate}")]
        public async Task<IActionResult> Schools(string laCode, SearchParameters parameters)
        {
            var result =
                from laName in GetLocalAuthorityName(laCode)
                from action in _schoolSearchController.Handle(
                    new EstablishmentScopeInfo(EstablishmentScopeType.LA, Optional<string>.Some(laCode)),
                    parameters,
                    [
                        new("All local authorities", "/local-authorities/"),
                        new(laName, $"/local-authority/{laCode}/")
                    ],
                    model => View(new SchoolSearchPageViewModel(
                        new PageViewModel(
                            model.BreadcrumbTrail,
                            model.PageTitle,
                            model.PageSubtitle
                        ),
                        model.Search,
                        model.Establishments
                    )),
                    new() {
                        [SchoolSearchSubActionType.AllSchools] = new() { Subtitle = laName }
                    }
                )
                select action;

            return await result
                .ToActionResult(_hostEnvironment);
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