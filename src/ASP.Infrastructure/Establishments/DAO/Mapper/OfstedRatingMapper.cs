namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class OfstedRatingMapper
{
    public static Core.Establishments.OfstedRating? MapToDomainEntityOfstedRating(this OfstedRatingDAO? ofstedRating)
    {
        if (ofstedRating == null) return null;  // Return null directly instead of an empty object
        return new Core.Establishments.OfstedRating()
        {
            Code = ofstedRating.Code,
            Name = ofstedRating.Name
        };
    }

    public static OfstedRatingDAO? MapToOfstedRatingDAO(this Core.Establishments.OfstedRating? ofstedRating)
    {
        if (ofstedRating == null) return null;  // Return null directly instead of an empty object
        return new OfstedRatingDAO(ofstedRating.Code, ofstedRating.Name);
    }
}