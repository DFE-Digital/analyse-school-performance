namespace ASP.Domain.Establishments.LinkedEstablishments;

public interface ILinkDescriptionGenerator
{
    string GenerateDescription(
        EstablishmentDetails establishment,
        DateTime? establishedDate,
        string linkTypeCode,
        List<LinkedEstablishment> linkedEstablishments);
}