namespace ASP.Domain.Establishments;

public class LinkedEstablishmentsResponse
{
    public string Urn { get; }
    public string Name { get; }
    public List<string> LinkedUrns { get; }
    public List<LinkResponse> Links { get; }

    public LinkedEstablishmentsResponse(
        string urn, 
        string name, 
        List<string> linkedUrns, 
        List<LinkResponse> links)
    {
        Urn = urn;
        Name = name;
        LinkedUrns = linkedUrns;
        Links = links;
    }
}