namespace ASP.Api.Client.Schools;

public class SchoolsGetLinkedSchoolsResponse
{
    public List<string> LinkedUrns { get; set; } = new List<string>();
    public List<SchoolLink> Links { get; set; } = new List<SchoolLink>();
}