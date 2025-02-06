namespace ASP.Api.Client.Templating;

public record UpdateContentTemplateRequest(string ContentTemplateId, string? Revision, ContentTemplate ContentTemplate);