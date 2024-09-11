namespace ASP.Application.UseCases.Establishments.DTO;

public class EstablishmentSuggestionDTO
{
    public string Urn { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Address { get; set; }
    public string? Laestab { get; set; }
}