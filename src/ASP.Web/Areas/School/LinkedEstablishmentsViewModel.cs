using ASP.Api.Client.Schools;

namespace ASP.Web.Areas.School;

public class LinkedEstablishmentsViewModel
{
    public string Urn { get; }
    public List<SchoolLink> Links { get; }
    public Func<string, string?> CreateSchoolUrl { get; }

    public LinkedEstablishmentsViewModel(string urn, List<SchoolLink> links, Func<string, string?> createSchoolUrl)
    {
        Urn = urn;
        Links = links;
        CreateSchoolUrl = createSchoolUrl;
    }
}