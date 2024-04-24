using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace ASP.Web.Features
{
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["action_displayname"] = context.ActionContext.ActionDescriptor.DisplayName;
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.ActionContext.ActionDescriptor is ControllerActionDescriptor actionDescriptor
                && actionDescriptor.Properties.TryGetValue("feature", out var value) && value is string featureName)
            {
                foreach (var location in viewLocations)
                {
                    yield return location.Replace("{3}", featureName);
                }
            } else
            {
                foreach (var location in viewLocations)
                {
                    yield return location;
                }
            }
        }
    }
}