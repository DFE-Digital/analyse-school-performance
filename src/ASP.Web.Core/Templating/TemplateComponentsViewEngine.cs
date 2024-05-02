using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ASP.Web.Core.Templating
{
    public class TemplateComponentsViewEngine : IViewEngine
    {
        private readonly IViewEngine _existingViewEngine;
        private readonly TemplateComponentOptions _options;

        public TemplateComponentsViewEngine(IViewEngine existingViewEngine, IOptions<TemplateComponentOptions> options)
        {
            _existingViewEngine = existingViewEngine;
            _options = options.Value;
        }

        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
        {
            var newViewName = viewName;
            if (context is ViewContext viewContext &&
                (viewContext.View.Path.StartsWith("/Features/ContentTemplates") || _options.ComponentLocations.Any(p => viewContext.View.Path.StartsWith(p))))
            {
                if (viewName.Contains("EditorTemplates/"))
                {
                    newViewName = viewName.Replace("EditorTemplates/", "") + "/Edit";
                }
                else
                {
                    newViewName = viewName + "/View";
                }
            }

            var result = _existingViewEngine.FindView(context, newViewName, isMainPage);
            return result;
        }

        public ViewEngineResult GetView(string? executingFilePath, string viewPath, bool isMainPage)
        {
            var result = _existingViewEngine.GetView(executingFilePath, viewPath, isMainPage);
            return result;
        }
    }

    public class TemplateComponentOptions
    {
        private readonly HashSet<string> _componentLocations = new HashSet<string>();
        private readonly List<Assembly> _componentAssemblies = new List<Assembly>();

        public ICollection<string> ComponentLocations => _componentLocations;
        public ICollection<Assembly> ComponentAssemblies => _componentAssemblies;
    }
}