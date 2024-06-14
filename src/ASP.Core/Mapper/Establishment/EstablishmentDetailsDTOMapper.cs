using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class EstablishmentDetailsDTOMapper
{
    public static EstablishmentDetailsDTO MapToEstablishmentDetailsDTO(this EstablishmentDetails details)
    {
        return new EstablishmentDetailsDTO(
        )
        {
            Urn = details.Urn,
            Name = details.Name,
            IsPrimary = details.IsPrimary,
            IsSecondary = details.IsSecondary,
            IsPost16 = details.IsPost16,
            Address = details.Address.MapToAddressDTO(),
            OfstedRating = details.OfstedRating.MapToOfstedRatingDTO(),
            OfstedLastInspectionDate = details.OfstedLastInspectionDate,
            ReligiousDenomination = details.ReligiousDenomination.MapToReligiousDenominationDTO(),
            AdmissionsPolicy = details.AdmissionsPolicy.MapToAdmissionsPolicyDTO(),
            LocalAuthority = details.LocalAuthority.MapToLocalAuthorityDTO(),
            HeadTeacher = details.HeadTeacher.MapToHeadTeacherDTO(),
            AgeRange = details.AgeRange.MapToAgeRangeDTO(),
            EstablishmentType = details.EstablishmentType.MapToEstablishmentTypeDTO(),
            Gender = details.Gender.MapToGenderDTO(),
            ResourcedProvisionType = details.ResourcedProvisionType.MapToResourcedProvisionTypeDTO(),
            NoOfPupils = details.NoOfPupils,
            Laestab = details.Laestab,
            IsDeleted = details.IsDeleted
        };
    }
}