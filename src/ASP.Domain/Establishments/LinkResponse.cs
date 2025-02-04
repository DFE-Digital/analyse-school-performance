namespace ASP.Domain.Establishments;

public class LinkResponse
{
    public DateTime? Date { get; }
    public LinkType? LinkType { get; }
    public List<LinkedEstablishment>? Establishments { get; }
    public string Description { get; }

    public LinkResponse(
        DateTime? date, 
        LinkType? linkType, 
        List<LinkedEstablishment>? establishments, 
        string description)
    {
        Date = date;
        LinkType = linkType;
        Establishments = establishments;
        Description = description;
    }
}