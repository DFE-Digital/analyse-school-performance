namespace ASP.Api.Client.Schools;

public class SchoolDetails
{
    public required string Urn { get; set; }
    public required string Laestab { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string EducationPhase { get; set; }
    public required string EstablishmentType { get; set; }
    public required string Gender { get; set; }
    public required string HeadTeacher { get; set; }
    public required string AgeRange { get; set; }
    public required string ReligiousDenomination { get; set; }
    public required string AdmissionsPolicy { get; set; }
    public required string ResourcedProvisionType { get; set; }
    public required string Diocese { get; set; }
    public required string NoOfPupils { get; set; }
    public LookupValueWithCode? LocalAuthority { get; set; }
    public string? MultiAcademyTrust { get; set; }
}