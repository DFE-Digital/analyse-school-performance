namespace ASP.Domain.Schools.Details
{
    public class EstablishmentDetails
    {
        public string EstablishmentType { get; }
        public string Gender { get; set; }
        public string HeadTeacher { get; }
        public string AgeRange { get; }
        public string ReligiousDenomination { get; }
        public string AdmissionsPolicy { get; }
        public string ResourcedProvisionType { get; }
        public string NoOfPupils { get; }

        public EstablishmentDetails(
            string establishmentType,
            string gender,
            string headTeacher,
            string ageRange,
            string religiousDenomination,
            string admissionsPolicy,
            string resourcedProvisionType,
            string noOfPupils)
        {
            EstablishmentType = establishmentType;
            Gender = gender;
            HeadTeacher = headTeacher;
            AgeRange = ageRange;
            ReligiousDenomination = religiousDenomination;
            AdmissionsPolicy = admissionsPolicy;
            ResourcedProvisionType = resourcedProvisionType;
            NoOfPupils = noOfPupils;
        }
    }
}
