namespace ASP.Api.Client.Schools;

public class SchoolLink
{
    public string? Date { get; set; }
    public LookupValueWithCode? LinkType { get; set; } = null;
    public List<LinkedSchool> Establishments { get; set; } = new List<LinkedSchool>();
    public string Description { get; set; } = "";
}