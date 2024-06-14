using ASP.Core.DTO;
using ASP.Core.DTO.Establishment;

namespace ASP.Core.Search;

public class EstablishmentDetailsSearchResultDTO : IEducationPhase
{
    public string Urn { get; set; }
    public string Name { get; set; }
    public bool? IsPrimary { get; set; }
    public bool? IsSecondary { get; set; }
    public bool? IsPost16 { get; set; }
    public AddressDTO Address { get; set; }
    public OfstedRatingDTO OfstedRating { get; set; }
    public DateTime? OfstedLastInspectionDate { get; set; }
    public string? LaEstab { get; set; }
    public bool IsDeleted { get; set; }
}