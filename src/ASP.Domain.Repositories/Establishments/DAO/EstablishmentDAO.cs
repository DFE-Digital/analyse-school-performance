namespace ASP.Domain.Repositories.Establishments.DAO
{
    public class EstablishmentDAO
    {
        public string Urn { get; }
        public string Name { get; }
        public bool? IsPrimary { get; }
        public bool? IsSecondary { get; }
        public bool? IsPost16 { get; }
        public AddressDAO? Address { get; }
        public EstablishmentTypeDAO? EstablishmentType { get; }
        public GenderDAO? Gender { get; }
        public LocalAuthorityDAO? LocalAuthority { get; }
        public HeadTeacherDAO? HeadTeacher { get; }
        public AgeRangeDAO? AgeRange { get; }
        public ReligiousDenominationDAO? ReligiousDenomination { get; }
        public AdmissionsPolicyDAO? AdmissionsPolicy { get; }
        public ResourcedProvisionTypeDAO? ResourcedProvisionType { get; }
        public DioceseDAO? Diocese { get; }
        public MultiAcademyTrustDAO? MultiAcademyTrust { get; }
        public List<LinkDAO>? Links { get; }
        public int? NoOfPupils { get; }
        public bool IsDeleted { get; }
        public string? Laestab { get; }
        public bool IsVisible { get; set; }
        public DateTime? OpenDate { get; }
        public DateTime? CloseDate { get; }

        public EstablishmentDAO(string urn,
            string name,
            bool? isPrimary,
            bool? isSecondary,
            bool? isPost16,
            AddressDAO? address,
            EstablishmentTypeDAO? establishmentType,
            GenderDAO? gender,
            LocalAuthorityDAO? localAuthority,
            HeadTeacherDAO? headTeacher,
            AgeRangeDAO? ageRange,
            ReligiousDenominationDAO? religiousDenomination,
            AdmissionsPolicyDAO? admissionsPolicy,
            ResourcedProvisionTypeDAO? resourcedProvisionType,
            DioceseDAO? diocese,
            MultiAcademyTrustDAO? multiAcademyTrust,
            List<LinkDAO>? links,
            int? noOfPupils, bool isDeleted,
            string? laestab, bool isVisible,
            DateTime? openDate, DateTime? closeDate)
        {
            Urn = urn;
            Name = name;
            IsPrimary = isPrimary;
            IsSecondary = isSecondary;
            IsPost16 = isPost16;
            Address = address;
            EstablishmentType = establishmentType;
            Gender = gender;
            LocalAuthority = localAuthority;
            HeadTeacher = headTeacher;
            AgeRange = ageRange;
            ReligiousDenomination = religiousDenomination;
            AdmissionsPolicy = admissionsPolicy;
            ResourcedProvisionType = resourcedProvisionType;
            Diocese = diocese;
            MultiAcademyTrust = multiAcademyTrust;
            Links = links;
            NoOfPupils = noOfPupils;
            IsDeleted = isDeleted;
            Laestab = laestab;
            IsVisible = isVisible;
            OpenDate = openDate;
            CloseDate = closeDate;
        }
    }
}