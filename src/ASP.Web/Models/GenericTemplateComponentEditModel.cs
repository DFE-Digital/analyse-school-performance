using ASP.Core.PageContent;

namespace ASP.Web.Models
{
    public class GenericTemplateComponentEditModel : TemplateComponentEditModel
    {
        public GenericTemplateComponentEditModel()
            : base()
        {
        }

        public GenericTemplateComponentEditModel(PageContentTemplateView contentTemplate) 
            : base(contentTemplate)
        {
        }
    }
}
