using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class OfstedRatingDTOMapper
{
    public static OfstedRatingDTO MapToOfstedRatingDTO(this OfstedRating? ofstedRating)
    {
        if (ofstedRating == null) return new OfstedRatingDTO();
        return new OfstedRatingDTO()
        {
            Code = ofstedRating.Code,
            Name = ofstedRating.Name
        };
    }
}