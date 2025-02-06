namespace ASP.Api.Client.Templating;

public record ContentTemplate(bool IsPublished, string PageTitle, dynamic PageContent, List<TemplateComponent> Views);