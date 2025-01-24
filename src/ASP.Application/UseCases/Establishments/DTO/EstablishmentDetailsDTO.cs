namespace ASP.Application.UseCases.Establishments.DTO;

public class EstablishmentDetailsDTO
{
    public string Urn { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Address { get; set; }
    public string? EducationPhase { get; set; }
    public EstablishmentTypeDTO? EstablishmentType { get; set; }
    public GenderDTO? Gender { get; set; }
    public LocalAuthorityDTO? LocalAuthority { get; set; }
    public HeadTeacherDTO? HeadTeacher { get; set; }
    public AgeRangeDTO? AgeRange { get; set; }
    public ReligiousDenominationDTO? ReligiousDenomination { get; set; }
    public AdmissionsPolicyDTO? AdmissionsPolicy { get; set; }
    public ResourcedProvisionTypeDTO? ResourcedProvisionType { get; set; }
    public int? NoOfPupils { get; set; }
    public string? Laestab { get; set; }
}