using ASP.Core.DTO.Establishment;

namespace ASP.Core.Search;

public class EstablishmentDetailsSearchResultDTO
{
    public string Urn { get; set; }
    public string Name { get; set; }
    public string? EducationPhase { get; set; }
    public string? Address { get; set; }
    public OfstedRatingDTO? OfstedRating { get; set; }
    public string? Laestab { get; set; }
}