namespace ASP.Infrastructure.Repositories.Establishments.DAO.Mapper;

public static class AgeRangeMapper
{
    public static Domain.Establishments.AgeRange? MapToDomainEntityAgeRange(this AgeRangeDAO? ageRange)
    {
        if (ageRange == null) return null;  // Return null directly instead of an empty object

        return new Domain.Establishments.AgeRange(
            ageRange.Low,
            ageRange.High
        );
    }

    public static AgeRangeDAO? MapToAgeRangeDAO(this Domain.Establishments.AgeRange? ageRange)
    {
        if (ageRange == null) return null;  // Return null directly instead of an empty object

        return new AgeRangeDAO(
            ageRange.Low,
            ageRange.High
        );
    }
}