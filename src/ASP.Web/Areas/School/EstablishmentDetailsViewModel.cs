using ASP.Core.Establishments;

namespace ASP.Web.Areas.School
{
    public class EstablishmentDetailsViewModel
    {
        public string Urn { get; set; } = ""; 
        public string Name { get; set; } = "";
        public bool IsPrimary { get; }
        public bool IsSecondary { get; }
        public bool Is16to18Establishment { get; }
        public Address Address { get; set; } = default!;
        public EstablishmentType EstablishmentType { get; set; } = default!;
        public string PhaseOfEducation { get; set; } = "";
        public Gender Gender { get; set; } = default!;
        public OfstedRating OfstedRating { get; set; } = default!;
        public DateOnly LastInspectionDate { get; set; } = default!;
        public LocalAuthority LocalAuthority { get; set; } = default!;
        public HeadTeacher HeadTeacher { get; set; } = default!;
        public AgeRange AgeRange { get; set; } = default!;
        public ReligiousDenomination ReligiousDenomination { get; set; } = default!;
        public AdmissionsPolicy AdmissionsPolicy { get; set; } = default!;
        public ResourcedProvisionType ResourcedProvisionType { get; set; } = default!;
        public int NoOfPupils { get; set; }
        public bool IsDeleted { get; set; }
    }
}