using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("server-error/{statusCode}")]
        public static void ServerError(string statusCode)
        {
            // if (statusCode == (int)HttpStatusCode.InternalServerError)
            // {
            throw new Exception();
            // }
        }
    }
}