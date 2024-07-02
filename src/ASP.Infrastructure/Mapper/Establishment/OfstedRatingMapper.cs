using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class OfstedRatingMapper
{
    public static Core.Establishments.OfstedRating? MapToDomainEntityOfstedRating(this OfstedRating? ofstedRating)
    {
        if (ofstedRating == null) return null;  // Return null directly instead of an empty object
        return new Core.Establishments.OfstedRating()
        {
            Code = ofstedRating.Code,
            Name = ofstedRating.Name
        };
    }
    
    public static OfstedRating? MapToOfstedRatingDAO(this Core.Establishments.OfstedRating? ofstedRating)
    {
        if (ofstedRating == null) return null;  // Return null directly instead of an empty object
        return new OfstedRating(ofstedRating.Code, ofstedRating.Name);
    }
}