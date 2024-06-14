using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class AgeRangeMapper
{
    public static Core.Establishments.AgeRange MapToDomainEntityAgeRange(this AgeRange? ageRange)
    {
        if (ageRange == null) return new Core.Establishments.AgeRange();
        
        return new Core.Establishments.AgeRange()
        {
            High = ageRange.High,
            Low = ageRange.Low
        };
    }
    
    public static AgeRange MapToAgeRangeDAO(this Core.Establishments.AgeRange ageRange)
    {
        return new AgeRange(ageRange.Low, ageRange.High);
    }
}