using ASP.Core.DTO.Establishment;

namespace ASP.Web.Areas.Shared.OfstedRating;

public class OfstedRatingModel
{
    public string OfstedLastInspectionDate { get; set; } = string.Empty;
    
    public OfstedRatingDTO? OfstedRating { get; set; }

    public string Urn { get; set; } = null!;

}