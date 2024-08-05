namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class AgeRangeMapper
{
    public static Core.Establishments.AgeRange? MapToDomainEntityAgeRange(this AgeRangeDAO? ageRange)
    {
        if (ageRange == null) return null;  // Return null directly instead of an empty object

        return new Core.Establishments.AgeRange()
        {
            High = ageRange.High,
            Low = ageRange.Low
        };
    }

    public static AgeRangeDAO? MapToAgeRangeDAO(this Core.Establishments.AgeRange? ageRange)
    {
        if (ageRange == null) return null;  // Return null directly instead of an empty object

        return new AgeRangeDAO(ageRange.Low, ageRange.High);
    }
}