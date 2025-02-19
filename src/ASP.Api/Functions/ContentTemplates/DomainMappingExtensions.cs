namespace ASP.Api.Functions.ContentTemplates;

public static class DomainMappingExtensions
{
    public static List<Client.ContentTemplates.ContentTemplate> ForApiClient(this List<Domain.Templating.ContentTemplate> templates)
    {
        return templates.Select(t => t.ForApiClient()).ToList();
    }

    public static Client.ContentTemplates.ContentTemplate ForApiClient(this Domain.Templating.ContentTemplate template)
    {
        return new Client.ContentTemplates.ContentTemplate(
            template.IsPublished,
            template.PageTitle,
            template.PageContent,
            template.Views.Select(v => v.ForApiClient()).ToList());
    }

    public static Client.ContentTemplates.TemplateComponent ForApiClient(this Domain.Templating.TemplateComponent component)
    {
        return new Client.ContentTemplates.TemplateComponent(
            component.ViewId,
            component.ViewContent,
            component.ViewModel,
            component.ChildViews.Select(v => v.ForApiClient()).ToList());
    }
}
