using ASP.Core.Establishments;

namespace ASP.Application.UseCases.Establishments.DTO.Mapper;

public static class AgeRangeDTOMapper
{
    public static AgeRangeDTO? MapToAgeRangeDTO(this AgeRange? ageRange)
    {
        if (ageRange == null) return null;  // Return null directly instead of an empty object
        return new AgeRangeDTO()
        {
            High = ageRange.High,
            Low = ageRange.Low
        };
    }
}