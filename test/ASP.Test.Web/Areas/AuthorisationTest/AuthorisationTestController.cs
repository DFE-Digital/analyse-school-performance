using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Test.Web.Areas.AuthorisationTest
{
    [Area("AuthorisationTest")]
    [Route("named-data")]
    public class AuthorisationTestController : Controller
    {
        public AuthorisationTestController()
        {
        }

        [Authorize(Policy = "ASP DfE Named")]
        [HttpGet("get-named-data-by-policy")]
        public ActionResult GetNamedDataByPolicy()
        {
            return Content("<p>Named data visible</p>", "text/html");
        }

        [Authorize()]
        [HttpGet("get-named-data-by-claim")]
        public ActionResult GetNamedDataByClaim()
        {
            return User.Claims.Any(x => x.Value == "ASP DfE Named")
                ? Content("<p>Named data visible</p>", "text/html")
                : Content("<p>Named data not visible</p>", "text/html");
        }
    }
}
