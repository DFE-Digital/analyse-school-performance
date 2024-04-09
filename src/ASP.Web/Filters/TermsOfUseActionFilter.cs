using ASP.Web.Enums;
using ASP.Web.Constants;
using ASP.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASP.Web.Filters
{
    public class TermsOfUseActionFilter : ActionFilterAttribute
    {
        private readonly string _redirectURL = "/help/accept-terms-of-use";
        private readonly ICookieProvider _cookieProvider;
        public TermsOfUseActionFilter(ICookieProvider cookieProvider)
        {
            _cookieProvider = cookieProvider ??
                throw new ArgumentNullException(nameof(cookieProvider));
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? acceptedTermsValue = _cookieProvider?.GetCookie(CookieKeys.AcceptedTermsOfUse);

            if (IsCookieAccepted(acceptedTermsValue))
                return;

            if (IsCookieRejected(acceptedTermsValue))
            {
                RedirectUserToTermsOfUsePage(context);
                return;
            }

            // If the cookie is not set/does not exist, set the cookie to Rejected and redirect
            _cookieProvider?.SetCookie(CookieKeys.AcceptedTermsOfUse, TermsOfUse.Rejected.ToString());

            RedirectUserToTermsOfUsePage(context);
        }



        private TermsOfUse ParseTermsOfUse(string? acceptedTermsValue)
        {
            if (Enum.TryParse(acceptedTermsValue, out TermsOfUse result))
                return result;

            return TermsOfUse.NotSet;
        }

        private bool IsCookieAccepted(string? acceptedTermsValue)
        {
            return ParseTermsOfUse(acceptedTermsValue) == TermsOfUse.Accepted;
        }

        private bool IsCookieRejected(string? acceptedTermsValue)
        {
            return ParseTermsOfUse(acceptedTermsValue) == TermsOfUse.Rejected;
        }

        private void RedirectUserToTermsOfUsePage(ActionExecutingContext context)
        {
            string originalRoute = context.HttpContext.Request.Path;

            // set query string if it is not the home route, this is so user can be redirected once terms are accepted
            string redirectUrl = originalRoute == "/" ? _redirectURL : $"{_redirectURL}?ref-url={originalRoute}";

            context.Result = new RedirectResult(redirectUrl);
        }
    }
}
