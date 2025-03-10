using ASP.Domain.Schools.LinkedSchools;

namespace ASP.Domain.Schools.UseCases.GetLinkedSchools;

public class GetLinkedSchoolsResponse
{
    public IReadOnlyCollection<SchoolUrn> LinkedUrns { get; }
    public IReadOnlyCollection<LinkedSchoolsLink> Links { get; }

    public GetLinkedSchoolsResponse(
        IReadOnlyCollection<SchoolUrn> linkedUrns,
        IReadOnlyCollection<LinkedSchoolsLink> links)
    {
        LinkedUrns = linkedUrns;
        Links = links;
    }
}