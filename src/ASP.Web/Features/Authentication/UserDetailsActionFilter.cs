using ASP.Core.Authorization;
using ASP.Infrastructure.Dsi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ASP.Web.Features.Authentication
{
    public class UserDetailsActionFilter : ActionFilterAttribute
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        public UserDetailsActionFilter(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                var dsiConfiguration = _configuration.GetSection(DsiConstants.DsiSection);
                var user = context.HttpContext.User;

                string firstName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? "";
                string lastName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? "";

                controller.ViewData["UserProfileUrl"] = dsiConfiguration[DsiConstants.DsiProfileUrl];
                controller.ViewData["UserName"] = $"{firstName} {lastName}";

                if (_hostEnvironment.IsDevelopment())
                {
                    controller.ViewData["UserRole"] = user.Role();
                }
            }
        }
    }
}