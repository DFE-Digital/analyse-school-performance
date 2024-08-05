namespace ASP.Infrastructure.Establishments.DAO
{
    public class EstablishmentDetailsDAO
    {
        public string Urn { get; }
        public string Name { get; }
        public bool? IsPrimary { get; }
        public bool? IsSecondary { get; }
        public bool? IsPost16 { get; }
        public AddressDAO? Address { get; }
        public EstablishmentTypeDAO? EstablishmentType { get; }
        public GenderDAO? Gender { get; }
        public OfstedRatingDAO? OfstedRating { get; }
        public DateTime? OfstedLastInspectionDate { get; }
        public LocalAuthorityDAO? LocalAuthority { get; }
        public HeadTeacherDAO? HeadTeacher { get; }
        public AgeRangeDAO? AgeRange { get; }
        public ReligiousDenominationDAO? ReligiousDenomination { get; }
        public AdmissionsPolicyDAO? AdmissionsPolicy { get; }
        public ResourcedProvisionTypeDAO? ResourcedProvisionType { get; }
        public int? NoOfPupils { get; }
        public bool IsDeleted { get; }
        public string? Laestab { get; }
        public bool IsVisible { get; set; }

        public EstablishmentDetailsDAO(string urn,
            string name,
            bool? isPrimary,
            bool? isSecondary,
            bool? isPost16,
            AddressDAO? address,
            EstablishmentTypeDAO? establishmentType,
            GenderDAO? gender,
            OfstedRatingDAO? ofstedRating,
            DateTime? ofstedLastInspectionDate,
            LocalAuthorityDAO? localAuthority,
            HeadTeacherDAO? headTeacher,
            AgeRangeDAO? ageRange,
            ReligiousDenominationDAO? religiousDenomination,
            AdmissionsPolicyDAO? admissionsPolicy,
            ResourcedProvisionTypeDAO? resourcedProvisionType,
            int? noOfPupils, bool isDeleted,
            string? laestab, bool isVisible)
        {
            Urn = urn;
            Name = name;
            IsPrimary = isPrimary;
            IsSecondary = isSecondary;
            IsPost16 = isPost16;
            Address = address;
            EstablishmentType = establishmentType;
            Gender = gender;
            OfstedRating = ofstedRating;
            OfstedLastInspectionDate = ofstedLastInspectionDate;
            LocalAuthority = localAuthority;
            HeadTeacher = headTeacher;
            AgeRange = ageRange;
            ReligiousDenomination = religiousDenomination;
            AdmissionsPolicy = admissionsPolicy;
            ResourcedProvisionType = resourcedProvisionType;
            NoOfPupils = noOfPupils;
            IsDeleted = isDeleted;
            Laestab = laestab;
            IsVisible = isVisible;
        }
    }
}
