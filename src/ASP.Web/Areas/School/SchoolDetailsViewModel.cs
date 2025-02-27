using ASP.Api.Client;
using ASP.Api.Client.Schools;

namespace ASP.Web.Areas.School
{
    public class SchoolDetailsViewModel
    {
        public required string Urn { get; set; }
        public required string Laestab { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string EducationPhase { get; set; }
        public required string EstablishmentType { get; set; }
        public required string Gender { get; set; }
        public required string HeadTeacher { get; set; }
        public required string AgeRange { get; set; }
        public required string ReligiousDenomination { get; set; }
        public required string AdmissionsPolicy { get; set; }
        public required string ResourcedProvisionType { get; set; }
        public required string NoOfPupils { get; set; }
        public required string Diocese { get; set; }
        public LookupValueWithCode? LocalAuthority { get; set; }
        public string? MultiAcademyTrust { get; set; }

        public static SchoolDetailsViewModel FromSchoolDetails(SchoolDetails schoolDetails)
        {
            return new SchoolDetailsViewModel
            {
                Urn = schoolDetails.Urn,
                Name = schoolDetails.Name,
                EducationPhase = schoolDetails.EducationPhase,
                Address = schoolDetails.Address,
                EstablishmentType = schoolDetails.EstablishmentType,
                Gender = schoolDetails.Gender,
                HeadTeacher = schoolDetails.HeadTeacher,
                AgeRange = schoolDetails.AgeRange,
                ReligiousDenomination = schoolDetails.ReligiousDenomination,
                AdmissionsPolicy = schoolDetails.AdmissionsPolicy,
                ResourcedProvisionType = schoolDetails.ResourcedProvisionType,
                NoOfPupils = schoolDetails.NoOfPupils,
                Laestab = schoolDetails.Laestab,
                Diocese = schoolDetails.Diocese,
                LocalAuthority = schoolDetails.LocalAuthority,
                MultiAcademyTrust = schoolDetails.MultiAcademyTrust
            };
        }
    }
}