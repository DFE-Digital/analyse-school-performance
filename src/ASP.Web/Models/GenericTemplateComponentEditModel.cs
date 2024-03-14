using ASP.Core.Templating;

namespace ASP.Web.Models
{
    public class GenericTemplateComponentEditModel : TemplateComponentEditModel
    {
        public GenericTemplateComponentEditModel()
            : base()
        {
        }

        public GenericTemplateComponentEditModel(TemplateComponent contentTemplate) 
            : base(contentTemplate)
        {
        }
    }
}
