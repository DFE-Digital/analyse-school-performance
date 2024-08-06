namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class EstablishmentDetailsMapper
{
    public static Core.Establishments.EstablishmentDetails MapToEstablishmentDetails(
        this EstablishmentDetailsDAO details)
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
            details.IsDeleted,
            details.Laestab,
            details.IsVisible
        );
    }

    public static EstablishmentDetailsDAO MapToEstablishmentDetailsDAO(
        this Core.Establishments.EstablishmentDetails details)
    {
        return new EstablishmentDetailsDAO(
            details.Urn, 
            details.Name, 
            details.IsPrimary, 
            details.IsSecondary,
            details.IsPost16,
            details.Address.MapToAddressDAO(), 
            details.EstablishmentType.MapToEstablishmentTypeDAO(),
            details.Gender.MapToGenderDAO(), 
            details.OfstedRating.MapToOfstedRatingDAO(),
            details.OfstedLastInspectionDate, 
            details.LocalAuthority.MapToLocalAuthorityDAO(),
            details.HeadTeacher.MapToHeadTeacherDAO(), 
            details.AgeRange.MapToAgeRangeDAO(),
            details.ReligiousDenomination.MapToReligiousDenominationDAO(),
            details.AdmissionsPolicy.MapToAdmissionsPolicyDAO(),
            details.ResourcedProvisionType.MapToResourcedProvisionTypeDAO(), 
            details.NoOfPupils, 
            details.IsDeleted,
            details.Laestab, 
            details.IsVisible
        );
    }
}