using ASP.Core.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Test.Web.Areas.AuthorizationTest
{
    [Area("AuthorizationTest")]
    [Route("named-data")]
    public class AuthorizationTestController : Controller
    {
        public AuthorizationTestController()
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
            return User.HasRole(Role.DfeNamed)
                ? Content("<p>Named data visible</p>", "text/html")
                : Content("<p>Named data not visible</p>", "text/html");
        }
    }
}
