using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ASP.Web.Models
{
    public class TemplateComponentEditModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            var templateComponentEditModelType = typeof(TemplateComponentEditModel);

            if (context.Metadata.ModelType != templateComponentEditModelType)
            {
                return null;
            }

            var editModelTypes = templateComponentEditModelType.Assembly.GetTypes()
                .Where(t => t != templateComponentEditModelType && t.IsAssignableTo(templateComponentEditModelType));

            var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();
            foreach (var type in editModelTypes)
            {
                var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
            }

            return new TemplateComponentEditModelBinder(binders);
        }
    }
}
