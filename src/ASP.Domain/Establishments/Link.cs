namespace ASP.Domain.Establishments;

public class Link
{
    public DateTime? EstablishedDate { get; }
    public LinkType? LinkType { get; }
    public string LinkedUrn { get; }

    public Link(DateTime? establishedDate, LinkType? linkType, string linkedUrn)
    {
        EstablishedDate = establishedDate;
        LinkType = linkType;
        LinkedUrn = linkedUrn;
    }
}