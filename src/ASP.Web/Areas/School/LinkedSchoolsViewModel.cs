using ASP.Api.Client.Schools;

namespace ASP.Web.Areas.School;

public class LinkedSchoolsViewModel
{
    public string Urn { get; }
    public List<SchoolLink> Links { get; }
    public Func<string, string?> CreateSchoolUrl { get; }

    public LinkedSchoolsViewModel(string urn, List<SchoolLink> links, Func<string, string?> createSchoolUrl)
    {
        Urn = urn;
        Links = links;
        CreateSchoolUrl = createSchoolUrl;
    }
}