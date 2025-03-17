using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace ASP.Web.Core.Templating
{
    public class TemplateComponentLocationExpander : IViewLocationExpander
    {
        private readonly TemplateComponentOptions _options;

        public TemplateComponentLocationExpander(IOptions<TemplateComponentOptions> options)
        {
            _options = options.Value;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if (context.ActionContext is ViewContext viewContext)
            {
                if (viewContext.View.Path.StartsWith("/Features/ContentTemplates"))
                {
                    context.Values["component_path"] = "/Features/ContentTemplates";
                }
                else 
                {
                    foreach(var path in _options.ComponentLocations)
                    {
                        if(viewContext.View.Path.StartsWith(path))
                        {
                            context.Values["component_path"] = path;
                            break;
                        }
                    }
                }
            }
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            foreach (var location in viewLocations)
            {
                yield return location;
            }

            if (context.Values.ContainsKey("component_path"))
            {
                foreach (var path in _options.ComponentLocations)
                {
                    yield return path + "/{0}" + RazorViewEngine.ViewExtension;
                }
            }
        }
    }
}