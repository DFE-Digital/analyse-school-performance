namespace ASP.Domain.Repositories.Schools;

public class LinkDao
{
    public DateTime? EstablishedDate { get; }
    public LookupValueDao? LinkType { get; }
    public string LinkedUrn { get; }

    public LinkDao(DateTime? establishedDate, LookupValueDao? linkType, string linkedUrn)
    {
        EstablishedDate = establishedDate;
        LinkType = linkType;
        LinkedUrn = linkedUrn;
    }
}