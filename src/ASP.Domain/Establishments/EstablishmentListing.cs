namespace ASP.Domain.Establishments
{
    public class EstablishmentListing
    {
        public string Urn { get; }
        public string Name { get; }
        public EducationPhase EducationPhase { get; }
        public Address? Address { get; }
        public string? Laestab { get; }

        public EstablishmentListing(
            string urn,
            string name,
            EducationPhase educationPhase, 
            Address? address,
            string? laestab)
        {
            Urn = urn;
            Name = name;
            EducationPhase = educationPhase;
            Address = address;
            Laestab = laestab;
        }
    }
}