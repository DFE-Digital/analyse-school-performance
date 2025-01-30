using ASP.Domain.Templating;

namespace ASP.Web.Core.Templating
{
    public class GenericTemplateComponentEditModel : TemplateComponentEditModel
    {
        public GenericTemplateComponentEditModel()
            : base()
        {
        }

        public GenericTemplateComponentEditModel(TemplateComponent contentTemplate, ITemplateComponentEditModelFactory editModelFactory)
            : base(contentTemplate, editModelFactory)
        {
        }
    }
}
