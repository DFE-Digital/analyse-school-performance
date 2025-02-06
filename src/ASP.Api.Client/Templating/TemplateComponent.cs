namespace ASP.Api.Client.Templating;

public record TemplateComponent(string ViewId, dynamic ViewContent, dynamic ViewModel, List<TemplateComponent> ChildViews);
