namespace ASP.Api.Client.Establishments;

public class EstablishmentDetails
{
    public string Urn { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Address { get; set; }
    public string? EducationPhase { get; set; }
    public LookupValueWithCode? EstablishmentType { get; set; }
    public LookupValueWithCode? Gender { get; set; }
    public LookupValueWithCode? LocalAuthority { get; set; }
    public HeadTeacher? HeadTeacher { get; set; }
    public AgeRange? AgeRange { get; set; }
    public LookupValueWithCode? ReligiousDenomination { get; set; }
    public LookupValueWithCode? AdmissionsPolicy { get; set; }
    public LookupValueWithCode? ResourcedProvisionType { get; set; }
    public int? NoOfPupils { get; set; }
    public string? Laestab { get; set; }
    public LookupValueWithId? MultiAcademyTrust { get; set; }
    public LookupValueWithCode? Diocese { get; set; }
}