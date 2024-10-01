using ASP.Core.Authorization;
using ASP.Infrastructure.Dsi;
using ASP.Web.Core.Environment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ASP.Web.Features.Authentication
{
    public class UserDetailsActionFilter : ActionFilterAttribute
    {
        private readonly DsiOidcOptions _options;
        private readonly IHostEnvironment _hostEnvironment;

        public UserDetailsActionFilter(IOptions<DsiOidcOptions> options, IHostEnvironment hostEnvironment)
        {
            _options = (options ?? throw new ArgumentNullException(nameof(options)))
                .Value;
            _hostEnvironment = hostEnvironment;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                var user = context.HttpContext.User;

                string firstName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? "";
                string lastName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? "";

                controller.ViewData["UserProfileUrl"] = _options.ProfileUrl;
                controller.ViewData["UserName"] = $"{firstName} {lastName}";

                if (_hostEnvironment.IsLocalDevelopment() || _hostEnvironment.IsDevelopment() || _hostEnvironment.IsTest())
                {
                    controller.ViewData["UserRole"] = user.Role();
                }
            }
        }
    }
}