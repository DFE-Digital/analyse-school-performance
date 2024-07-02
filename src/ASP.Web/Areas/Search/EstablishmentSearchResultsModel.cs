using ASP.Core.DTO.Establishment;
using ASP.Core.Search;

namespace ASP.Web.Areas.Search;

public class EstablishmentSearchResultsModel
{
    public string Urn { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Address { get; set; }
    public string? PhaseOfEducation { get; set; } = "";
    public OfstedRatingDTO? OfstedRating { get; set; }
    public string LaEstab { get; set; } = "";

    public static List<EstablishmentSearchResultsModel> FromEstablishmentDetails(
        IEnumerable<EstablishmentDetailsSearchResultDTO> establishmentDetails)
    {
        var result = establishmentDetails.Select(x => new EstablishmentSearchResultsModel
            {
                Urn = x.Urn,
                Name = x.Name,
                PhaseOfEducation = x.EducationPhase,
                Address = x.Address,
                OfstedRating = x.OfstedRating,
                LaEstab = x.LaEstab ?? "No data available"
            }
        ).ToList();

        return result;
    }
}