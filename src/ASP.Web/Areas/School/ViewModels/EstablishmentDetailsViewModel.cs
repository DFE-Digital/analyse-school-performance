using ASP.Core.Establishments;

namespace ASP.Web.Areas.School.ViewModels
{
    public class EstablishmentDetailsViewModel
    {
        public string Urn { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsPrimary { get; }
        public bool IsSecondary { get; }
        public bool Is16to18Establishment { get; }
        public Address? Address { get; set; }
        public EstablishmentType? EstablishmentType { get; set; }
        public string PhaseOfEducation { get; set; } = "";
        public Gender? Gender { get; set; }
        public OfstedRating? OfstedRating { get; set; }
        public DateTime? OfstedLastInspectionDate { get; set; }
        public LocalAuthority? LocalAuthority { get; set; }
        public HeadTeacher? HeadTeacher { get; set; }
        public AgeRange? AgeRange { get; set; }
        public ReligiousDenomination? ReligiousDenomination { get; set; }
        public AdmissionsPolicy? AdmissionsPolicy { get; set; }
        public ResourcedProvisionType? ResourcedProvisionType { get; set; }
        public int? NoOfPupils { get; set; }
        
        public bool IsDeleted { get; set; }

        public static EstablishmentDetailsViewModel FromEstablishmentDetails(EstablishmentDetails establishmentDetails)
        {
            return new EstablishmentDetailsViewModel {
                Urn = establishmentDetails.Urn,
                Name = establishmentDetails.Name,
                PhaseOfEducation = GetPhaseOfEducation(establishmentDetails),
                Address = establishmentDetails.Address,
                EstablishmentType = establishmentDetails.EstablishmentType,
                Gender = establishmentDetails.Gender,
                OfstedRating = establishmentDetails.OfstedRating,
                OfstedLastInspectionDate = establishmentDetails.OfstedLastInspectionDate,
                LocalAuthority = establishmentDetails.LocalAuthority,
                HeadTeacher = establishmentDetails.HeadTeacher,
                AgeRange = establishmentDetails.AgeRange,
                ReligiousDenomination = establishmentDetails.ReligiousDenomination,
                AdmissionsPolicy = establishmentDetails.AdmissionsPolicy,
                ResourcedProvisionType = establishmentDetails.ResourcedProvisionType,
                NoOfPupils = establishmentDetails.NoOfPupils,
                IsDeleted = establishmentDetails.IsDeleted
            };
        }

        private static string GetPhaseOfEducation(EstablishmentDetails establishmentDetails)
        {
            var phaseOfEducation = "";
            if (establishmentDetails.IsPrimary) phaseOfEducation = "Primary";
            if (establishmentDetails.IsSecondary) phaseOfEducation = "Secondary";
            if (establishmentDetails.IsPost16) phaseOfEducation = "16 to 18";
            return phaseOfEducation;
        }
    }
}