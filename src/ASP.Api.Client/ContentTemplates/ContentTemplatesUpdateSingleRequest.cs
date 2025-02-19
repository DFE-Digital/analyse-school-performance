namespace ASP.Api.Client.ContentTemplates;

public record ContentTemplatesUpdateSingleRequest(string ContentTemplateId, string? Revision, ContentTemplate ContentTemplate);