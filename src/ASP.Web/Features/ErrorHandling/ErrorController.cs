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
        public IActionResult Error(int? statusCode = null)
        {
            // The errorModel is reused for both Error pages. Even though no properties of the model are utilized,
            // it appears necessary for the view to initialize it with a specific property, particularly for the page not found error page.
            var errorModel = new ErrorViewModel { ErrorCode = HttpContext.TraceIdentifier };

            if (statusCode.HasValue && statusCode == 404)
            {
                return View("~/Features/ErrorHandling/PageNotFoundError.cshtml", errorModel);
            }

            return View("~/Features/ErrorHandling/ServerError.cshtml", errorModel);
        }
    }
}
