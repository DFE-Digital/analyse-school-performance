using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace ASP.Web.Core.Templating
{
    public class TemplateComponentEditModelBinderProvider : IModelBinderProvider
    {
        private static readonly Type _templateComponentEditModelType = typeof(TemplateComponentEditModel);
        private readonly TemplateComponentEditModelTypeLocator _editModelTypeLocator;

        public TemplateComponentEditModelBinderProvider(TemplateComponentEditModelTypeLocator editModelTypeLocator)
        {
            _editModelTypeLocator = editModelTypeLocator;
        }

        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType != _templateComponentEditModelType)
            {
                return null;
            }

            var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();
            foreach (var type in _editModelTypeLocator.GetEditModelTypes())
            {
                var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
            }

            return new TemplateComponentEditModelBinder(binders, _editModelTypeLocator);
        }
    }
}
