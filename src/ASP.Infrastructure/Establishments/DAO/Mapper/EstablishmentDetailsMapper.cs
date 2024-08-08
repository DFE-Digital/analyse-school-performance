namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class EstablishmentDetailsMapper
{
    public static Core.Establishments.EstablishmentDetails MapToEstablishmentDetails(
        this EstablishmentDAO details)
    {
        return new Core.Establishments.EstablishmentDetails(
            details.Urn,
            details.Name,
            details.IsPrimary,
            details.IsSecondary,
            details.IsPost16,
            details.Address.MapToDomainEntityAddress(),
            details.EstablishmentType.MapToDomainEntityEstablishmentType(),
            details.Gender.MapToDomainEntityGender(),
            details.OfstedRating.MapToDomainEntityOfstedRating(),
            details.OfstedLastInspectionDate,
            details.LocalAuthority.MapToDomainEntityLocalAuthority(),
            details.HeadTeacher.MapToDomainEntityHeadTeacher(),
            details.AgeRange.MapToDomainEntityAgeRange(),
            details.ReligiousDenomination.MapToDomainEntityReligiousDenomination(),
            details.AdmissionsPolicy.MapToDomainEntityAdmissionsPolicy(),
            details.ResourcedProvisionType.MapToDomainEntityResourcedProvisionType(),
            details.NoOfPupils,
            details.Laestab
        );
    }
}