using ASP.Application.Utilities;
using ASP.Core.DTO.Establishment;
using ASP.Core.Search;

namespace ASP.Web.Areas.Search;

public class EstablishmentSearchResultsModel
{
    public string Urn { get; set; } = "";
    public string Name { get; set; } = "";
    public AddressDTO? Address { get; set; }
    public string PhaseOfEducation { get; set; } = "";
    public OfstedRatingDTO? OfstedRating { get; set; }
    public string OfstedLastInspectionDate { get; set; } = string.Empty;
    public string LaEstab { get; set; } = "";

    public static List<EstablishmentSearchResultsModel> FromEstablishmentDetails(IEnumerable<EstablishmentDetailsSearchResultDTO> establishmentDetails)
    {
        var result = establishmentDetails.Select(x => new EstablishmentSearchResultsModel
            {
                Urn = x.Urn,
                Name = x.Name,
                PhaseOfEducation = EducationPhase.GetPhaseOfEducation(x),
                Address = x.Address,
                OfstedRating = x.OfstedRating,
                OfstedLastInspectionDate = x.OfstedLastInspectionDate != null ?
                    x.OfstedLastInspectionDate.Value.ToString("dd MMMM yyyy") : "No data available",
                LaEstab = x.LaEstab ?? "No data available"
            }
        ).ToList();

        return result;
    }
}