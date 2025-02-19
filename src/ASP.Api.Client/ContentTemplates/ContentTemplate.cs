namespace ASP.Api.Client.ContentTemplates;

public record ContentTemplate(bool IsPublished, string PageTitle, dynamic PageContent, List<TemplateComponent> Views);