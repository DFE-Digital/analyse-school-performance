namespace ASP.Api.Client.Establishments;

public class EstablishmentLink
{
    public string? Date { get; set; }
    public LookupValueWithCode? LinkType { get; set; } = null;
    public List<LinkedEstablishment> Establishments { get; set; } = new List<LinkedEstablishment>();
    public string Description { get; set; } = "";
}