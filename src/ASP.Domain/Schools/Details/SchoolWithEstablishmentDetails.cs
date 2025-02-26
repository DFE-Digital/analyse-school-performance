namespace ASP.Domain.Schools.Details
{
    public class SchoolWithEstablishmentDetails : School
    {
        public SchoolWithEstablishmentDetails(
            SchoolUrn urn,
            LAEstabCode laEstab,
            string name,
            EducationPhase educationPhase,
            Address? address,
            DateTime? openDate,
            DateTime? closeDate,
            LocalAuthority? localAuthority,
            MultiAcademyTrust? multiAcademyTrust,
            Diocese? diocese,
            EstablishmentDetails establishmentDetails)
            : base(urn, laEstab, name, educationPhase, address, openDate, closeDate, localAuthority, multiAcademyTrust, diocese)
        {
            EstablishmentDetails = establishmentDetails;
        }

        public EstablishmentDetails EstablishmentDetails { get; }
    }
}
