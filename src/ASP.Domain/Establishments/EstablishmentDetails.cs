using ASP.Domain.Establishments.LinkedEstablishments;

namespace ASP.Domain.Establishments
{
    public class EstablishmentDetails
    {
        public string Urn { get; }
        public string Name { get; }
        public EducationPhase EducationPhase { get; }
        public Address? Address { get; }
        public EstablishmentType? EstablishmentType { get; }
        public Gender? Gender { get; set; }
        public LocalAuthority? LocalAuthority { get; }
        public HeadTeacher? HeadTeacher { get; }
        public AgeRange? AgeRange { get; }
        public ReligiousDenomination? ReligiousDenomination { get; }
        public AdmissionsPolicy? AdmissionsPolicy { get; }
        public ResourcedProvisionType? ResourcedProvisionType { get; }
        public List<Link>? Links { get; }
        public int? NoOfPupils { get; }
        public string? Laestab { get; }
        public DateTime? OpenDate { get; }
        public DateTime? CloseDate { get; }

        public EstablishmentDetails(string urn, string name, EducationPhase educationPhase, Address? address, EstablishmentType? establishmentType,
            Gender? gender, LocalAuthority? localAuthority, HeadTeacher? headTeacher, AgeRange? ageRange,
            ReligiousDenomination? religiousDenomination, AdmissionsPolicy? admissionsPolicy,
            ResourcedProvisionType? resourcedProvisionType, List<Link>? links,
            int? noOfPupils, string? laestab, DateTime? openDate, DateTime? closeDate)
        {
            Urn = urn;
            Name = name;
            EducationPhase = educationPhase;
            Address = address;
            EstablishmentType = establishmentType;
            Gender = gender;
            LocalAuthority = localAuthority;
            HeadTeacher = headTeacher;
            AgeRange = ageRange;
            ReligiousDenomination = religiousDenomination;
            AdmissionsPolicy = admissionsPolicy;
            ResourcedProvisionType = resourcedProvisionType;
            Links = links;
            NoOfPupils = noOfPupils;
            Laestab = laestab;
            OpenDate = openDate;
            CloseDate = closeDate;
        }
    }
}
