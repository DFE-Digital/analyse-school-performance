using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class GenderDTOMapper
{
    public static GenderDTO MapToGenderDTO(this Gender? gender)
    {
        if (gender == null) return new GenderDTO();
        
        return new GenderDTO()
        {
            Code = gender.Code,
            Name = gender.Name
        };
    }
}