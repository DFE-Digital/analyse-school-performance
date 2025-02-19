using ASP.Api.Client.ContentTemplates;

namespace ASP.Web.Core.Templating
{
    public interface ITemplateComponentEditModelFactory
    {
        TemplateComponentEditModel CreateTemplateComponentEditModel(TemplateComponent v);
    }
}
