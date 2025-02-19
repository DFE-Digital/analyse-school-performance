namespace ASP.Api.Client.ContentTemplates;

public record TemplateComponent(string ViewId, dynamic ViewContent, dynamic ViewModel, List<TemplateComponent> ChildViews);
