namespace ASP.Domain.Repositories.Establishments.DAO;

public class LinkDAO
{
    public DateTime? EstablishedDate { get; }
    public LinkTypeDAO? LinkType { get; }
    public string LinkedUrn { get; }

    public LinkDAO(DateTime? establishedDate, LinkTypeDAO? linkType, string linkedUrn)
    {
        EstablishedDate = establishedDate;
        LinkType = linkType;
        LinkedUrn = linkedUrn;
    }
}