using ASP.Web.Features.TermsOfUse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASP.Web.Features.ErrorHandling
{
    [Route("error")]
    [ServiceFilter<TermsOfUseActionFilter>]
    public class ErrorController : Controller
    {
        private readonly ErrorHandlingOptions _options;

        public ErrorController(IOptions<ErrorHandlingOptions> options)
        {
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("")]
        public IActionResult Error()
        {
            var errorModel = new ErrorViewModel { 
                ErrorCode = HttpContext.TraceIdentifier
            };

            if (_options.ShowStackTrace && HttpContext.Items["Exception"] is Exception ex)
            {
                errorModel.ErrorMessage = ex.Message;
                errorModel.StackTrace = ex.StackTrace;

                while(ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    errorModel.ErrorMessage += " " + ex.Message;
                    errorModel.StackTrace += " " + ex.StackTrace;
                }
            }

            return View("~/Features/ErrorHandling/ServerError.cshtml", errorModel);
        }

        [HttpGet("accessdenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
