namespace ASP.Core.DTO.Establishment;

public class EstablishmentDetailsDTO : IEducationPhase
{
    public string Urn { get; set; }
    public string Name { get; set; }
    public bool? IsPrimary { get; set; }
    public bool? IsSecondary { get; set; }
    public bool? IsPost16 { get; set; }
    public AddressDTO Address { get; set; }
    public EstablishmentTypeDTO EstablishmentType { get; set; }
    public GenderDTO Gender { get; set;}
    public OfstedRatingDTO OfstedRating { get; set; }
    public DateTime? OfstedLastInspectionDate { get; set; }
    public LocalAuthorityDTO LocalAuthority { get; set; }
    public HeadTeacherDTO HeadTeacher { get; set;}
    public AgeRangeDTO AgeRange { get; set; }
    public ReligiousDenominationDTO ReligiousDenomination { get; set; }
    public AdmissionsPolicyDTO AdmissionsPolicy { get; set; }
    public ResourcedProvisionTypeDTO ResourcedProvisionType { get; set; }
    public int? NoOfPupils { get; set; }
    public bool IsDeleted { get; set; }
    public string? Laestab { get; set; }
}