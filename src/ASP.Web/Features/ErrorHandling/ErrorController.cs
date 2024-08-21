using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASP.Web.Features.ErrorHandling
{
    [Route("error")]
    [ServiceFilter<TermsOfUseActionFilter>]
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly ErrorHandlingOptions _options;

        public ErrorController(IOptions<ErrorHandlingOptions> options)
        {
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("servererror")]
        public IActionResult ServerError()
        {
            var errorModel = new ErrorViewModel { 
                ErrorCode = HttpContext.TraceIdentifier
            };

            if (HttpContext.Items["ErrorMessage"] is string errorMessage)
            {
                errorModel.ErrorMessage = errorMessage;
            }

            if (_options.ShowStackTrace && HttpContext.Items["StackTrace"] is string stackTrace)
            {
                errorModel.StackTrace = stackTrace;
            }

            return View(errorModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("pagenotfound")]
        public IActionResult PageNotFound()
        {
            var errorModel = new ErrorViewModel {
                ErrorCode = HttpContext.TraceIdentifier
            };

            if (HttpContext.Items["ErrorMessage"] is string errorMessage)
            {
                errorModel.ErrorMessage = errorMessage;
            }

            if (_options.ShowStackTrace && HttpContext.Items["StackTrace"] is string stackTrace)
            {
                errorModel.StackTrace = stackTrace;
            }

            return View(errorModel);
        }

        [HttpGet("accessdenied")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
