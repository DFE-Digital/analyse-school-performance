namespace ASP.Api.Client.Schools;

public class SchoolListing
{
    public string Urn { get; set; } = "";
    public string Name { get; set; } = "";
    public string? EducationPhase { get; set; }
    public string? Address { get; set; }
    public string? Laestab { get; set; }
}