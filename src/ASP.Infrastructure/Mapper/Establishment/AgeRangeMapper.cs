using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class AgeRangeMapper
{
    public static Core.Establishments.AgeRange? MapToDomainEntityAgeRange(this AgeRange? ageRange)
    {
        if (ageRange == null) return null;  // Return null directly instead of an empty object
        
        return new Core.Establishments.AgeRange()
        {
            High = ageRange.High,
            Low = ageRange.Low
        };
    }
    
    public static AgeRange? MapToAgeRangeDAO(this Core.Establishments.AgeRange? ageRange)
    {
        if (ageRange == null) return null;  // Return null directly instead of an empty object
        
        return new AgeRange(ageRange.Low, ageRange.High);
    }
}