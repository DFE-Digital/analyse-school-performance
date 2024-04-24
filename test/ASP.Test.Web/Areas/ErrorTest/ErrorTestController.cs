using Microsoft.AspNetCore.Mvc;

namespace ASP.Test.Web.Areas.ErrorTest
{
    // Test controller for testing exception handling which is added as an Application Part to the test assembly:
    //   services.AddMvc()
    //     .AddApplicationPart(typeof(ErrorTestController).Assembly)
    //     .AddControllersAsServices();
    //
    [Area("ErrorTest")]
    [Route("error-test")]
    public class ErrorTestController : Controller
    {
        [HttpGet("throw-exception")]
        public void ThrowException()
        {
            throw new Exception();
        }
        
        [HttpGet("page-not-found-error/{errorMessage}")]
        public IActionResult PageNotFoundError(string errorMessage)
        {
            return NotFound(errorMessage);
        }
    }
}