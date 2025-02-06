using ASP.Domain.Establishments.LinkedEstablishments;

namespace ASP.Domain.Establishments.UseCases.GetLinkedEstablishments;

public class GetLinkedEstablishmentsResponse
{
    public string Urn { get; }
    public string Name { get; }
    public List<string> LinkedUrns { get; }
    public List<EstablishmentLink> Links { get; }

    public GetLinkedEstablishmentsResponse(
        string urn,
        string name,
        List<string> linkedUrns,
        List<EstablishmentLink> links)
    {
        Urn = urn;
        Name = name;
        LinkedUrns = linkedUrns;
        Links = links;
    }
}