using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Features.ErrorHandling
{
    [Route("error")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("")]
        public IActionResult Error()
        {
            var errorModel = new ErrorViewModel { 
                ErrorCode = HttpContext.TraceIdentifier
            };

            return View("~/Features/ErrorHandling/ServerError.cshtml", errorModel);
        }
    }
}
