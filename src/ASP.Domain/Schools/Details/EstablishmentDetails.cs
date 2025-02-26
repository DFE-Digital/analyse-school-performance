namespace ASP.Domain.Schools.Details
{
    public class EstablishmentDetails
    {
        public LookupValue? EstablishmentType { get; }
        public LookupValue? Gender { get; set; }
        public HeadTeacher? HeadTeacher { get; }
        public AgeRange? AgeRange { get; }
        public LookupValue? ReligiousDenomination { get; }
        public LookupValue? AdmissionsPolicy { get; }
        public LookupValue? ResourcedProvisionType { get; }
        public int? NoOfPupils { get; }
        public DateTime? OpenDate { get; }
        public DateTime? CloseDate { get; }

        public EstablishmentDetails(
            LookupValue? establishmentType,
            LookupValue? gender,
            HeadTeacher? headTeacher,
            AgeRange? ageRange,
            LookupValue? religiousDenomination,
            LookupValue? admissionsPolicy,
            LookupValue? resourcedProvisionType,
            int? noOfPupils,
            DateTime? openDate,
            DateTime? closeDate)
        {
            EstablishmentType = establishmentType;
            Gender = gender;
            HeadTeacher = headTeacher;
            AgeRange = ageRange;
            ReligiousDenomination = religiousDenomination;
            AdmissionsPolicy = admissionsPolicy;
            ResourcedProvisionType = resourcedProvisionType;
            NoOfPupils = noOfPupils;
            OpenDate = openDate;
            CloseDate = closeDate;
        }
    }
}
