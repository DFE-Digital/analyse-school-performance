using Microsoft.AspNetCore.Mvc;
using ASP.Web.Features.Cookies;

namespace ASP.Web.Features.TermsOfUse
{
    [Route("terms-of-use")]
    public class TermsOfUseController : Controller
    {
        private readonly ICookieProvider _cookieProvider;

        public TermsOfUseController(ICookieProvider cookieProvider)
        {
            _cookieProvider = cookieProvider ??
                throw new ArgumentNullException(nameof(cookieProvider));
        }

        [HttpPost("accept")]
        public IActionResult Accept()
        {
            _cookieProvider.SetCookie(CookieKeys.AcceptedTermsOfUse, TermsOfUse.Accepted.ToString());

            // Read the "ref-url" query string parameter, this is set from the terms of use action filter
            string referer = HttpContext.Request.Query["ref-url"].ToString();

            return Redirect(string.IsNullOrEmpty(referer) ? "/" : referer);
        }
    }
}