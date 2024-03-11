using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ASP.Web.Models
{
    public class TemplateComponentEditModelBinder : IModelBinder
    {
        private Dictionary<Type, (ModelMetadata, IModelBinder)> _binders;

        public TemplateComponentEditModelBinder(Dictionary<Type, (ModelMetadata, IModelBinder)> binders)
        {
            _binders = binders;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var viewIdModelName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, "ViewId");
            var viewId = bindingContext.ValueProvider.GetValue(viewIdModelName).FirstValue;

            if(viewId == null)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            }

            IModelBinder modelBinder;
            ModelMetadata modelMetadata;

            var editModelType = TemplateComponentEditModel.FindEditModelType(_binders.Keys, viewId );
            (modelMetadata, modelBinder) = _binders[editModelType];

            var newBindingContext = DefaultModelBindingContext.CreateBindingContext(
                bindingContext.ActionContext,
                bindingContext.ValueProvider,
                modelMetadata,
                bindingInfo: null,
                bindingContext.ModelName);

            await modelBinder.BindModelAsync(newBindingContext);
            bindingContext.Result = newBindingContext.Result;

            if (newBindingContext.Result.IsModelSet)
            {
                // Setting the ValidationState ensures properties on derived types are correctly 
                bindingContext.ValidationState[newBindingContext.Result.Model] = new ValidationStateEntry {
                    Metadata = modelMetadata,
                };
            }
        }
    }
}
