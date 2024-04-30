using ASP.Core.Establishments;

namespace ASP.Infrastructure.Repositories
{
    public class EstablishmentDTO
    {
        public string Urn { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsPrimary { get; set; }
        public bool IsSecondary { get; set; }
        public bool Is16to18Establishment { get; set; }
        public Address Address { get; set; } = default!;
        public EstablishmentType EstablishmentType { get; set; } = default!;
        public Gender Gender { get; set; } = default!;
        public OfstedRating OfstedRating { get; set; } = default!;
        public DateOnly LastInspectionDate { get; set; } = default!;
        public LocalAuthority LocalAuthority { get; set; } = default!;
        public HeadTeacher HeadTeacher { get; set; } = default!;
        public AgeRange AgeRange { get; set; } = default!;
        public ReligiousDenomination ReligiousDenomination { get; set; } = default!;
        public AdmissionsPolicy AdmissionsPolicy { get; set; } = default!;
        public ResourcedProvisionType ResourcedProvisionType { get; set; } = default!;
        public int NoOfPupils { get; set; }
        public bool IsDeleted { get; set; } = false;

        internal EstablishmentDetails ToEstablishment()
        {
            return new EstablishmentDetails(
                Urn,
                Name,
                IsPrimary,
                IsSecondary,
                Is16to18Establishment,
                Address,
                EstablishmentType,
                Gender,
                OfstedRating,
                LastInspectionDate,
                LocalAuthority,
                HeadTeacher,
                AgeRange,
                ReligiousDenomination,
                AdmissionsPolicy,
                ResourcedProvisionType,
                NoOfPupils,
                IsDeleted
            );
        }

    }
}
