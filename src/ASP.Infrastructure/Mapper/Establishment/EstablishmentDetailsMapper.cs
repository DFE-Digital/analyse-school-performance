using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class EstablishmentDetailsMapper
{
    public static Core.Establishments.EstablishmentDetails MapToEstablishmentDetails(
        this EstablishmentDetailsDAO details)
    {
        return new Core.Establishments.EstablishmentDetails()
        {
            Urn = details.Urn,
            Name = details.Name,
            IsPrimary = details.IsPrimary,
            IsSecondary = details.IsSecondary,
            IsPost16 = details.IsPost16,
            Address = details.Address.MapToDomainEntityAddress(),
            OfstedRating = details.OfstedRating.MapToDomainEntityOfstedRating(),
            Gender = details.Gender.MapToDomainEntityGender(),
            LocalAuthority = details.LocalAuthority.MapToDomainEntityLocalAuthority(),
            EstablishmentType = details.EstablishmentType.MapToDomainEntityEstablishmentType(),
            HeadTeacher = details.HeadTeacher.MapToDomainEntityHeadTeacher(),
            AgeRange = details.AgeRange.MapToDomainEntityAgeRange(),
            ReligiousDenomination = details.ReligiousDenomination.MapToDomainEntityReligiousDenomination(),
            AdmissionsPolicy = details.AdmissionsPolicy.MapToDomainEntityAdmissionsPolicy(),
            ResourcedProvisionType = details.ResourcedProvisionType.MapToDomainEntityResourcedProvisionType(),
            OfstedLastInspectionDate = details.OfstedLastInspectionDate,
            NoOfPupils = details.NoOfPupils,
            Laestab = details.Laestab,
            IsDeleted = details.IsDeleted,
            IsVisible = details.IsVisible
        };
    }

    public static EstablishmentDetailsDAO MapToEstablishmentDetailsDAO(
        this Core.Establishments.EstablishmentDetails details)
    {
        return new EstablishmentDetailsDAO(details.Urn, details.Name, details.IsPrimary, details.IsSecondary,
            details.IsPost16,
            details.Address.MapToAddressDAO(), details.EstablishmentType.MapToEstablishmentTypeDAO(),
            details.Gender.MapToGenderDAO(), details.OfstedRating.MapToOfstedRatingDAO(),
            details.OfstedLastInspectionDate, details.LocalAuthority.MapToLocalAuthorityDAO(),
            details.HeadTeacher.MapToHeadTeacherDAO(), details.AgeRange.MapToAgeRangeDAO(),
            details.ReligiousDenomination.MapToReligiousDenominationDAO(),
            details.AdmissionsPolicy.MapToAdmissionsPolicyDAO(),
            details.ResourcedProvisionType.MapToResourcedProvisionTypeDAO(), details.NoOfPupils, details.IsDeleted,
            details.Laestab, details.IsVisible);
    }
}