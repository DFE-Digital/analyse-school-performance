namespace ASP.Core.Establishments
{
    public class EstablishmentDetails
    {
        public string Urn { get; set; } = default!;
        public string Name { get; set;} = default!;
        public bool? IsPrimary { get; set;}
        public bool? IsSecondary { get; set;}
        public bool? IsPost16 { get; set;}
        public Address? Address { get; set;}
        public EstablishmentType? EstablishmentType { get; set;}
        public Gender? Gender { get; set;}
        public OfstedRating? OfstedRating { get; set;}
        public DateTime? OfstedLastInspectionDate { get; set;}
        public LocalAuthority? LocalAuthority { get; set;}
        public HeadTeacher? HeadTeacher { get; set;}
        public AgeRange? AgeRange { get; set;}
        public ReligiousDenomination? ReligiousDenomination { get; set;}
        public AdmissionsPolicy? AdmissionsPolicy { get; set;}
        public ResourcedProvisionType? ResourcedProvisionType { get; set;}
        public int? NoOfPupils { get; set;}
        public bool IsDeleted { get; set;}
        public string? Laestab { get; set;}
        public bool IsVisible { get; set;}
    }
}
