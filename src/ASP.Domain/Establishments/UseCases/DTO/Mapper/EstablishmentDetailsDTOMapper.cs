namespace ASP.Domain.Establishments.UseCases.DTO.Mapper;

public static class EstablishmentDetailsDTOMapper
{
    public static EstablishmentDetailsDTO MapToEstablishmentDetailsDTO(this EstablishmentDetails details)
    {
        return new EstablishmentDetailsDTO() {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.MapToAddressDTO(),
            EducationPhase = EducationPhase.GetPhaseOfEducation(details.IsPrimary, details.IsSecondary, details.IsPost16),
            ReligiousDenomination = details.ReligiousDenomination.MapToReligiousDenominationDTO(),
            AdmissionsPolicy = details.AdmissionsPolicy.MapToAdmissionsPolicyDTO(),
            LocalAuthority = details.LocalAuthority.MapToLocalAuthorityDTO(),
            HeadTeacher = details.HeadTeacher.MapToHeadTeacherDTO(),
            AgeRange = details.AgeRange.MapToAgeRangeDTO(),
            EstablishmentType = details.EstablishmentType.MapToEstablishmentTypeDTO(),
            Gender = details.Gender.MapToGenderDTO(),
            ResourcedProvisionType = details.ResourcedProvisionType.MapToResourcedProvisionTypeDTO(),
            NoOfPupils = details.NoOfPupils,
            Laestab = details.Laestab
        };
    }
}