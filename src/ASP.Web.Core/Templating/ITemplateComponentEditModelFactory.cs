using ASP.Api.Client.Templating;

namespace ASP.Web.Core.Templating
{
    public interface ITemplateComponentEditModelFactory
    {
        TemplateComponentEditModel CreateTemplateComponentEditModel(TemplateComponent v);
    }
}
