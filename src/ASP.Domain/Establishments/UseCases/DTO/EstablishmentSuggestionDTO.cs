namespace ASP.Domain.Establishments.UseCases.DTO;

public class EstablishmentSuggestionDTO
{
    public string Urn { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Address { get; set; }
    public string? Laestab { get; set; }
}