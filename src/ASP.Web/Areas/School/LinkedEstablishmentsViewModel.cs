using ASP.Api.Client.Establishments;

namespace ASP.Web.Areas.School;

public class LinkedEstablishmentsViewModel
{
    public string Urn { get; }
    public List<EstablishmentLink> Links { get; }
    public Func<string, string?> CreateSchoolUrl { get; }

    public LinkedEstablishmentsViewModel(string urn, List<EstablishmentLink> links, Func<string, string?> createSchoolUrl)
    {
        Urn = urn;
        Links = links;
        CreateSchoolUrl = createSchoolUrl;
    }
}