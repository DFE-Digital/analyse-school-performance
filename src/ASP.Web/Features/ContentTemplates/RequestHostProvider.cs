using ASP.Web.Core.Templating;

namespace ASP.Web.Features.ContentTemplates
{
    public class RequestHostProvider : IRequestHostProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public RequestHostProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string RequestHost => _contextAccessor.HttpContext!.Request.Host.Host;
    }
}
