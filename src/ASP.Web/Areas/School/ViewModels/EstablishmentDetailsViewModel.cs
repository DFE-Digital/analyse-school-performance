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
    }
}