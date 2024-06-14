using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class AgeRangeDTOMapper
{
    public static AgeRangeDTO MapToAgeRangeDTO(this AgeRange? ageRange)
    {
        if (ageRange == null) return new AgeRangeDTO();
        return new AgeRangeDTO()
        {
           High = ageRange.High,
           Low = ageRange.Low
        };
    }
}