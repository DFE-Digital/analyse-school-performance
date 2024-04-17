using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ASP.Web.Controllers
{
    // Test controller for testing exception handling which is added as an Application Part to the test assembly:
    //   services.AddMvc()
    //     .AddApplicationPart(typeof(ErrorTestController).Assembly)
    //     .AddControllersAsServices();
    //
    [Route("error-test")]
    public class ErrorTestController : Controller
    {
        [HttpGet("throw-exception")]
        public void ThrowException()
        {
           throw new Exception();
        }
    }
}