using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASP.Web.Features.ApplicationServiceVersion
{
    public class CurrentVersionActionFilter : ActionFilterAttribute
    {
        private readonly ICurrentVersionProvider _currentVersionProvider;
        public CurrentVersionActionFilter(ICurrentVersionProvider currentVersionProvider)
        {
            _currentVersionProvider = currentVersionProvider ??
                throw new ArgumentNullException(nameof(currentVersionProvider));
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Controller? controller = context.Controller as Controller;

            string currentVersion = _currentVersionProvider.GetCurrentVersion();

            controller?.ViewData.Add("CurrentVersion", currentVersion);
        }
    }
}
