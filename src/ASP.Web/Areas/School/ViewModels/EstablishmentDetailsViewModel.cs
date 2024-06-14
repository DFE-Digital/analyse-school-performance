using ASP.Application.Utilities;
using ASP.Core.DTO.Establishment;

namespace ASP.Web.Areas.School.ViewModels
{
    public class EstablishmentDetailsViewModel
    {
        public string Urn { get; set; } = "";
        public string Name { get; set; } = "";
        public bool? IsPrimary { get; }
        public bool? IsSecondary { get; }
        public bool? Is16to18Establishment { get; }
        public AddressDTO? Address { get; set; }
        public EstablishmentTypeDTO? EstablishmentType { get; set; }
        public string PhaseOfEducation { get; set; } = "";
        public GenderDTO? Gender { get; set; }
        public OfstedRatingDTO? OfstedRating { get; set; }
        public string OfstedLastInspectionDate { get; set; } = string.Empty;
        public LocalAuthorityDTO? LocalAuthority { get; set; }
        public HeadTeacherDTO? HeadTeacher { get; set; }
        public AgeRangeDTO? AgeRange { get; set; }
        public ReligiousDenominationDTO? ReligiousDenomination { get; set; }
        public AdmissionsPolicyDTO? AdmissionsPolicy { get; set; }
        public ResourcedProvisionTypeDTO? ResourcedProvisionType { get; set; }
        public int? NoOfPupils { get; set; }
        public bool IsDeleted { get; set; }

        public static EstablishmentDetailsViewModel FromEstablishmentDetails(EstablishmentDetailsDTO establishmentDetailsDto)
        {
            return new EstablishmentDetailsViewModel {
                Urn = establishmentDetailsDto.Urn,
                Name = establishmentDetailsDto.Name,
                PhaseOfEducation = EducationPhase.GetPhaseOfEducation(establishmentDetailsDto),
                Address = establishmentDetailsDto.Address,
                EstablishmentType = establishmentDetailsDto.EstablishmentType,
                Gender = establishmentDetailsDto.Gender,
                OfstedRating = establishmentDetailsDto.OfstedRating,
                OfstedLastInspectionDate = establishmentDetailsDto.OfstedLastInspectionDate != null ?
                    establishmentDetailsDto.OfstedLastInspectionDate.Value.ToString("dd MMMM yyyy") : string.Empty,
                LocalAuthority = establishmentDetailsDto.LocalAuthority,
                HeadTeacher = establishmentDetailsDto.HeadTeacher,
                AgeRange = establishmentDetailsDto.AgeRange,
                ReligiousDenomination = establishmentDetailsDto.ReligiousDenomination,
                AdmissionsPolicy = establishmentDetailsDto.AdmissionsPolicy,
                ResourcedProvisionType = establishmentDetailsDto.ResourcedProvisionType,
                NoOfPupils = establishmentDetailsDto.NoOfPupils,
                IsDeleted = establishmentDetailsDto.IsDeleted
            };
        }
    }
}