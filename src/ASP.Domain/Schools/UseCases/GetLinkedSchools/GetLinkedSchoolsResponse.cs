using ASP.Domain.Schools.LinkedSchools;

namespace ASP.Domain.Schools.UseCases.GetLinkedSchools;

public class GetLinkedSchoolsResponse
{
    public List<SchoolUrn> LinkedUrns { get; }
    public List<LinkedSchoolsLink> Links { get; }

    public GetLinkedSchoolsResponse(
        List<SchoolUrn> linkedUrns,
        List<LinkedSchoolsLink> links)
    {
        LinkedUrns = linkedUrns;
        Links = links;
    }
}