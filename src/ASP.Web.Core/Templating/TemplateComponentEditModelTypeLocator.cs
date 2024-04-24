using Microsoft.Extensions.Options;
using System.Reflection;

namespace ASP.Web.Core.Templating
{
    public class TemplateComponentEditModelTypeLocator
    {
        private static readonly Type _templateComponentEditModelType = typeof(TemplateComponentEditModel);
        private readonly TemplateComponentOptions _options;

        public TemplateComponentEditModelTypeLocator(IOptions<TemplateComponentOptions> options)
        {
            _options = options.Value;
        }

        public IEnumerable<Type> GetEditModelTypes()
        {
            return _options.ComponentAssemblies
                .Append(_templateComponentEditModelType.Assembly)
                .SelectMany(a => a.GetTypes())
                .Where(t => t != _templateComponentEditModelType && t.IsAssignableTo(_templateComponentEditModelType))
                .ToList();
        }

        public Type FindEditModelType(string viewId)
        {
            var templateComponentEditModelType = typeof(TemplateComponentEditModel);

            return GetEditModelTypes()
                .FirstOrDefault(t => t.GetCustomAttribute<EditModelForAttribute>()?.ViewId == viewId)
                ?? typeof(GenericTemplateComponentEditModel);
        }
    }
}
