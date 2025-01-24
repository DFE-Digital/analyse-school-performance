using ASP.Application;
using ASP.Core.Optionality;
using ASP.Core.Scoping;
using ASP.Web.Features.Search;
using ASP.Web.Extensions;
using ASP.Web.Features.Authorization;
using ASP.Web.Features.TermsOfUse;
using ASP.Web.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace ASP.Web.Areas.School
{
    [Area("School")]
    [Route("schools")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [Authorize(Policy = Policy.AccessToAllSchools)]
    public class GenericSchoolsController : SchoolsController
    {
        private readonly SchoolSearchController _schoolSearchController;

        public GenericSchoolsController(
            IAspApiClient api,
            IHostEnvironment hostEnvironment,
            IOptions<SearchOptions> searchOptions
        ) : base(api, hostEnvironment)
        {
            _schoolSearchController = new SchoolSearchController(nameof(Schools), "GenericSchools", [], _api, searchOptions.Value,
                makeSchoolUrl: urn => Url.Action(nameof(GenericSchoolController.LandingPage), "GenericSchool", new { urn }));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            _schoolSearchController.BindContext(context);
        }

        [HttpGet($"{SchoolSearchController.SubRouteTemplate}")]
        public async Task<IActionResult> Schools(SearchParameters parameters)
        {
            var result = await _schoolSearchController.Handle(
                new ScopeInfo(ScopeType.All, Optional<string>.None),
                parameters,
                [],
                model => View(new SchoolSearchPageViewModel(
                    new PageViewModel(
                        model.BreadcrumbTrail,
                        model.PageTitle,
                        model.PageSubtitle
                    ),
                    model.Search,
                    model.Establishments
                ))
            );

            return result
                .ToActionResult(_hostEnvironment);
        }
    }
}