namespace ASP.Api.Client.Establishments;

public class GetLinkedEstablishmentsResponse
{
    public string Urn { get; set; } = "";
    public string Name { get; set; } = "";
    public List<string> LinkedUrns { get; set; } = new List<string>();
    public List<EstablishmentLink> Links { get; set; } = new List<EstablishmentLink>();
}