namespace ASP.Api.Client.Schools;

public record SchoolsGetAccessResponse(
    bool IsAccessibleInScope,
    bool IsAccessibleViaLinkedSchools);
