namespace ASP.Domain.Schools
{
    public class School
    {
        public School(
            SchoolUrn urn,
            LAEstabCode? laEstab,
            string name,
            EducationPhase educationPhase,
            Address? address,
            DateTime? openDate,
            DateTime? closeDate,
            LocalAuthority? localAuthority,
            MultiAcademyTrust? multiAcademyTrust,
            Diocese? diocese)
        {
            Urn = urn;
            LAEstab = laEstab;
            Name = name;
            EducationPhase = educationPhase;
            Address = address;
            OpenDate = openDate;
            CloseDate = closeDate;
            LocalAuthority = localAuthority;
            MultiAcademyTrust = multiAcademyTrust;
            Diocese = diocese;
        }

        public SchoolUrn Urn { get; }
        public LAEstabCode? LAEstab { get; }
        public string Name { get; }
        public EducationPhase EducationPhase { get; }
        public Address? Address { get; }
        public DateTime? OpenDate { get; }
        public DateTime? CloseDate { get; }
        public LocalAuthority? LocalAuthority { get; }
        public MultiAcademyTrust? MultiAcademyTrust { get; }
        public Diocese? Diocese { get; }
    }
}
