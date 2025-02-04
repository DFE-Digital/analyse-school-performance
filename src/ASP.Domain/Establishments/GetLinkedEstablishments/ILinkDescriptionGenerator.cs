namespace ASP.Domain.Establishments.GetLinkedEstablishments;

public interface ILinkDescriptionGenerator
{
    string GenerateDescription(
        EstablishmentDetails establishment,
        DateTime? establishedDate,
        string linkTypeCode,
        List<LinkedEstablishment> linkedEstablishments);
}