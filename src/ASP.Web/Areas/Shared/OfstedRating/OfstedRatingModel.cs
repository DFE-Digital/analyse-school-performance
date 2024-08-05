using ASP.Application.UseCases.Establishments.DTO;

namespace ASP.Web.Areas.Shared.OfstedRating;

public class OfstedRatingModel
{
    public OfstedRatingDTO? OfstedRating { get; set; }
    public string Urn { get; set; } = null!;
}