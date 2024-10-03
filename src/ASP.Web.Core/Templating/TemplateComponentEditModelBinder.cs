using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ASP.Web.Core.Templating
{
    public class TemplateComponentEditModelBinder : IModelBinder
    {
        private Dictionary<Type, (ModelMetadata, IModelBinder)> _binders;
        private readonly TemplateComponentEditModelTypeLocator _editModelTypeLocator;

        public TemplateComponentEditModelBinder(Dictionary<Type, (ModelMetadata, IModelBinder)> binders, TemplateComponentEditModelTypeLocator editModelTypeLocator)
        {
            _binders = binders;
            _editModelTypeLocator = editModelTypeLocator;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var viewIdModelName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, "ViewId");
            var viewId = bindingContext.ValueProvider.GetValue(viewIdModelName).FirstValue;

            if (viewId == null)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            }

            IModelBinder modelBinder;
            ModelMetadata modelMetadata;

            var editModelType = _editModelTypeLocator.FindEditModelType(viewId);
            (modelMetadata, modelBinder) = _binders[editModelType];

            var newBindingContext = DefaultModelBindingContext.CreateBindingContext(
                bindingContext.ActionContext,
                bindingContext.ValueProvider,
                modelMetadata,
                bindingInfo: null,
                bindingContext.ModelName);

            await modelBinder.BindModelAsync(newBindingContext);
            bindingContext.Result = newBindingContext.Result;

            if (newBindingContext.Result.IsModelSet && newBindingContext.Result.Model != null)
            {
                // Setting the ValidationState ensures properties on derived types are correctly 
                bindingContext.ValidationState[newBindingContext.Result.Model] = new ValidationStateEntry
                {
                    Metadata = modelMetadata,
                };
            }
        }
    }
}
