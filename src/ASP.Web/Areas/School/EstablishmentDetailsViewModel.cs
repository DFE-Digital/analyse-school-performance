using ASP.Api.Client;
using ASP.Api.Client.Schools;

namespace ASP.Web.Areas.School
{
    public class EstablishmentDetailsViewModel
    {
        public string Urn { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Address { get; set; }
        public LookupValueWithCode? EstablishmentType { get; set; }
        public string? PhaseOfEducation { get; set; } = "";
        public LookupValueWithCode? Gender { get; set; }
        public LookupValueWithCode? LocalAuthority { get; set; }
        public HeadTeacher? HeadTeacher { get; set; }
        public AgeRange? AgeRange { get; set; }
        public LookupValueWithCode? ReligiousDenomination { get; set; }
        public LookupValueWithCode? AdmissionsPolicy { get; set; }
        public LookupValueWithCode? ResourcedProvisionType { get; set; }
        public int? NoOfPupils { get; set; }
        public string? Laestab { get; set; }
        public LookupValueWithId? MultiAcademyTrust { get; set; }
        public LookupValueWithCode? Diocese { get; set; }

        public static EstablishmentDetailsViewModel FromEstablishmentDetails(SchoolDetails establishmentDetailsDto)
        {
            return new EstablishmentDetailsViewModel
            {
                Urn = establishmentDetailsDto.Urn,
                Name = establishmentDetailsDto.Name,
                PhaseOfEducation = establishmentDetailsDto.EducationPhase,
                Address = establishmentDetailsDto.Address,
                EstablishmentType = establishmentDetailsDto.EstablishmentType,
                Gender = establishmentDetailsDto.Gender,
                LocalAuthority = establishmentDetailsDto.LocalAuthority,
                HeadTeacher = establishmentDetailsDto.HeadTeacher,
                AgeRange = establishmentDetailsDto.AgeRange,
                ReligiousDenomination = establishmentDetailsDto.ReligiousDenomination,
                AdmissionsPolicy = establishmentDetailsDto.AdmissionsPolicy,
                ResourcedProvisionType = establishmentDetailsDto.ResourcedProvisionType,
                NoOfPupils = establishmentDetailsDto.NoOfPupils,
                Laestab = establishmentDetailsDto.Laestab,
                MultiAcademyTrust = establishmentDetailsDto.MultiAcademyTrust,
                Diocese = establishmentDetailsDto.Diocese
            };
        }
    }
}