using ASP.Domain.Templating;

namespace ASP.Web.Core.Templating
{
    public class TemplateComponentEditModelFactory: ITemplateComponentEditModelFactory
    {
        private readonly TemplateComponentEditModelTypeLocator _editModelTypeLocator;

        public TemplateComponentEditModelFactory(TemplateComponentEditModelTypeLocator editModelTypeLocator)
        {
            _editModelTypeLocator = editModelTypeLocator;
        }

        public TemplateComponentEditModel CreateTemplateComponentEditModel(TemplateComponent v)
        {
            var editModelType = _editModelTypeLocator.FindEditModelType(v.ViewId);

            return (TemplateComponentEditModel)Activator.CreateInstance(editModelType, v, this)!;
        }
    }
}
