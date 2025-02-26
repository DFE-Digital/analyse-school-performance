namespace ASP.Domain.Repositories.Schools
{
    public class EstablishmentDao
    {
        public string Urn { get; }
        public string Name { get; }
        public bool? IsPrimary { get; }
        public bool? IsSecondary { get; }
        public bool? IsPost16 { get; }
        public AddressDao? Address { get; }
        public LookupValueDao? EstablishmentType { get; }
        public LookupValueDao? Gender { get; }
        public LookupValueDao? LocalAuthority { get; }
        public HeadTeacherDao? HeadTeacher { get; }
        public AgeRangeDao? AgeRange { get; }
        public LookupValueDao? ReligiousDenomination { get; }
        public LookupValueDao? AdmissionsPolicy { get; }
        public LookupValueDao? ResourcedProvisionType { get; }
        public LookupValueDao? Diocese { get; }
        public MultiAcademyTrustDao? MultiAcademyTrust { get; }
        public List<LinkDao>? Links { get; }
        public int? NoOfPupils { get; }
        public bool IsDeleted { get; }
        public string? Laestab { get; }
        public bool IsVisible { get; set; }
        public DateTime? OpenDate { get; }
        public DateTime? CloseDate { get; }

        public EstablishmentDao(string urn,
            string name,
            bool? isPrimary,
            bool? isSecondary,
            bool? isPost16,
            AddressDao? address,
            LookupValueDao? establishmentType,
            LookupValueDao? gender,
            LookupValueDao? localAuthority,
            HeadTeacherDao? headTeacher,
            AgeRangeDao? ageRange,
            LookupValueDao? religiousDenomination,
            LookupValueDao? admissionsPolicy,
            LookupValueDao? resourcedProvisionType,
            LookupValueDao? diocese,
            MultiAcademyTrustDao? multiAcademyTrust,
            List<LinkDao>? links,
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