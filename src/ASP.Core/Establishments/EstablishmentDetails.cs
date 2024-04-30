namespace ASP.Core.Establishments
{
    public class EstablishmentDetails
    {
        public string Urn { get; }
        public string Name { get; }
        public bool IsPrimary { get; }
        public bool IsSecondary { get; }
        public bool IsPost16 { get; }
        public Address Address { get; }
        public EstablishmentType EstablishmentType { get; }
        public Gender Gender { get;}
        public OfstedRating OfstedRating { get; }
        public DateOnly LastInspectionDate { get; }
        public LocalAuthority LocalAuthority { get; }
        public HeadTeacher HeadTeacher { get;}
        public AgeRange AgeRange { get; }
        public ReligiousDenomination ReligiousDenomination { get; }
        public AdmissionsPolicy AdmissionsPolicy { get;}
        public ResourcedProvisionType ResourcedProvisionType { get; }
        public int NoOfPupils { get;}
        public bool IsDeleted { get; }
        public EstablishmentDetails(string urn, 
            string name, 
            bool isPrimary,
            bool isSecondary,
            bool isPost16,
            Address address, 
            EstablishmentType establishmentType,
            Gender gender,
            OfstedRating ofstedRating,
            DateOnly lastInspectionDate,
            LocalAuthority localAuthority,
            HeadTeacher headTeacher,
            AgeRange ageRange,
            ReligiousDenomination religiousDenomination,
            AdmissionsPolicy admissionsPolicy,
            ResourcedProvisionType resourcedProvisionType,
            int noOfPupils, bool isDeleted)
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
            LastInspectionDate = lastInspectionDate;
            LocalAuthority = localAuthority;
            HeadTeacher = headTeacher;
            AgeRange = ageRange;
            ReligiousDenomination = religiousDenomination;
            AdmissionsPolicy = admissionsPolicy;
            ResourcedProvisionType = resourcedProvisionType;
            NoOfPupils = noOfPupils;
            IsDeleted = isDeleted;
        }
    }
}
