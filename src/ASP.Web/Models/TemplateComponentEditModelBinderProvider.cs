using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ASP.Web.Models
{
    public class TemplateComponentEditModelBinderProvider : IModelBinderProvider
    {
        private static readonly Type _templateComponentEditModelType = typeof(TemplateComponentEditModel);
        private static List<Type> _editModelTypes = _templateComponentEditModelType.Assembly.GetTypes()
                .Where(t => t != _templateComponentEditModelType && t.IsAssignableTo(_templateComponentEditModelType))
                .ToList();

        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType != _templateComponentEditModelType)
            {
                return null;
            }

            var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();
            foreach (var type in _editModelTypes)
            {
                var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
            }

            return new TemplateComponentEditModelBinder(binders);
        }
    }
}
