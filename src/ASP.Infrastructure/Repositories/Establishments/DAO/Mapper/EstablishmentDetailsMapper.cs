namespace ASP.Infrastructure.Repositories.Establishments.DAO.Mapper;

public static class EstablishmentDetailsMapper
{
    public static Domain.Establishments.EstablishmentDetails MapToEstablishmentDetails(
        this EstablishmentDAO details)
    {
        return new Domain.Establishments.EstablishmentDetails(
            details.Urn,
            details.Name,
            details.IsPrimary,
            details.IsSecondary,
            details.IsPost16,
            details.Address.MapToDomainEntityAddress(),
            details.EstablishmentType.MapToDomainEntityEstablishmentType(),
            details.Gender.MapToDomainEntityGender(),
            details.LocalAuthority.MapToDomainEntityLocalAuthority(),
            details.HeadTeacher.MapToDomainEntityHeadTeacher(),
            details.AgeRange.MapToDomainEntityAgeRange(),
            details.ReligiousDenomination.MapToDomainEntityReligiousDenomination(),
            details.AdmissionsPolicy.MapToDomainEntityAdmissionsPolicy(),
            details.ResourcedProvisionType.MapToDomainEntityResourcedProvisionType(),
            details.Links.MapToDomainEntityLinks(),
            details.NoOfPupils,
            details.Laestab,
            details.OpenDate,
            details.CloseDate
        );
    }
}