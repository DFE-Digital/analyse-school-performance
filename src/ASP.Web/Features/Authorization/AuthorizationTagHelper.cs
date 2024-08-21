using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ASP.Web.Features.Authorization
{
    [HtmlTargetElement(Attributes = "authorization-policy")]
    public class AuthorizationTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationTagHelper(
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationService authorizationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
        }

        [HtmlAttributeName("authorization-policy")]
        public string? Policy { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Policy == null)
            {
                return;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return;
            }

            //To view content where the 'authorization-policy' attribute is used, you need to be signed in
            //with DSI and have the correct Role(s) assigned to you

            var check = (await _authorizationService.AuthorizeAsync(httpContext.User, Policy)).Succeeded;

            if (!check)
            {
                output.SuppressOutput();
            }
        }
    }
}
