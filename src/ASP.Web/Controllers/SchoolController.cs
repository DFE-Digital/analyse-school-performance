using ASP.Web.Filters;
using ASP.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Web;

namespace ASP.Web.Controllers
{
    [Route("school")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class SchoolController : Controller
    {
        private readonly ILogger<SchoolController> _logger;

        public SchoolController(ILogger<SchoolController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("my-school")]
        public async Task<ActionResult> SelectSchool(SelectSchoolViewModel viewModel)
        {
            if (IsPostRequest)
            {
                if (viewModel.Urn.HasValue)
                {
                    return RedirectToRoute(RouteSelectYear, new { id = viewModel.Urn, search = viewModel.Search });
                }

                ModelState.AddModelError("select-school-form", "Please choose a school");
            }

            viewModel.Establishments = await SearchEstablishmentsAsync(viewModel.Search);

            if (viewModel.HasSearchText)
            {
                if (!viewModel.Establishments.Any())
                {
                    return View("SearchNoResults", viewModel);
                }

                if (viewModel.Establishments.Length == 1)
                {
                    var urn = viewModel.Establishments.First().Urn;
                    _permissions.AssertUserHasAccessToSchoolOrLocalAuthority(urn);

                    if (SearchTextAnalyser.GetTextTokenType(viewModel.Search).EqualsAny(SearchTextTokenType.Urn, SearchTextTokenType.LAESTAB)
                        || viewModel.Establishments[0].Name.DoesEqual(viewModel.Search))
                    {
                        return RedirectToRoute(RouteSelectYear, new { id = urn, sa = -1 });
                    }
                }
            }

            return View(viewModel);
        }

        public bool IsPostRequest => Request.HttpMethod.DoesEqual(HttpMethod.Post.Method);
    }

    
}
