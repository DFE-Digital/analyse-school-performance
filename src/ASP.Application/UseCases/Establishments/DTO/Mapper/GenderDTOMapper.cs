using ASP.Core.Establishments;

namespace ASP.Application.UseCases.Establishments.DTO.Mapper;

public static class GenderDTOMapper
{
    public static GenderDTO? MapToGenderDTO(this Gender? gender)
    {
        if (gender == null) return null;  // Return null directly instead of an empty object

        return new GenderDTO()
        {
            Code = gender.Code,
            Name = gender.Name
        };
    }
}