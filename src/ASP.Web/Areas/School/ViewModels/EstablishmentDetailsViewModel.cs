using ASP.Core.DTO.Establishment;

namespace ASP.Web.Areas.School.ViewModels
{
    public class EstablishmentDetailsViewModel
    {
        public string Urn { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Address { get; set; }
        public EstablishmentTypeDTO? EstablishmentType { get; set; }
        public string? PhaseOfEducation { get; set; } = "";
        public GenderDTO? Gender { get; set; }
        public OfstedRatingDTO? OfstedRating { get; set; }
        public LocalAuthorityDTO? LocalAuthority { get; set; }
        public HeadTeacherDTO? HeadTeacher { get; set; }
        public AgeRangeDTO? AgeRange { get; set; }
        public ReligiousDenominationDTO? ReligiousDenomination { get; set; }
        public AdmissionsPolicyDTO? AdmissionsPolicy { get; set; }
        public ResourcedProvisionTypeDTO? ResourcedProvisionType { get; set; }
        public int? NoOfPupils { get; set; }

        public static EstablishmentDetailsViewModel FromEstablishmentDetails(EstablishmentDetailsDTO establishmentDetailsDto)
        {
            return new EstablishmentDetailsViewModel {
                Urn = establishmentDetailsDto.Urn,
                Name = establishmentDetailsDto.Name,
                PhaseOfEducation = establishmentDetailsDto.EducationPhase,
                Address = establishmentDetailsDto.Address,
                EstablishmentType = establishmentDetailsDto.EstablishmentType,
                Gender = establishmentDetailsDto.Gender,
                OfstedRating = establishmentDetailsDto.OfstedRating,
                LocalAuthority = establishmentDetailsDto.LocalAuthority,
                HeadTeacher = establishmentDetailsDto.HeadTeacher,
                AgeRange = establishmentDetailsDto.AgeRange,
                ReligiousDenomination = establishmentDetailsDto.ReligiousDenomination,
                AdmissionsPolicy = establishmentDetailsDto.AdmissionsPolicy,
                ResourcedProvisionType = establishmentDetailsDto.ResourcedProvisionType,
                NoOfPupils = establishmentDetailsDto.NoOfPupils
            };
        }
    }
}