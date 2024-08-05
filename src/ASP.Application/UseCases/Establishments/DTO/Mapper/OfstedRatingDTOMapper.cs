using ASP.Core.Establishments;

namespace ASP.Application.UseCases.Establishments.DTO.Mapper;

public static class OfstedRatingDTOMapper
{
    public static OfstedRatingDTO? MapToOfstedRatingDTO(this OfstedRating? ofstedRating,
        DateTime? lastInspectionDate)
    {
        if (ofstedRating == null) return null;  // Return null directly instead of an empty OfstedRatingDTO
        return new OfstedRatingDTO()
        {
            Code = ofstedRating.Code,
            Name = ofstedRating.Name,
            LastInspected = lastInspectionDate?.ToString("dd MMMM yyyy") ?? null
        };
    }
}