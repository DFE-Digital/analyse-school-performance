using ASP.Core.Establishments;

namespace ASP.Application.UseCases.Establishments.DTO.Mapper;

public static class ReligiousDenominationDTOMapper
{
    public static ReligiousDenominationDTO? MapToReligiousDenominationDTO(this ReligiousDenomination? religiousDenomination)
    {
        if (religiousDenomination == null) return null;  // Return null directly instead of an empty object
        return new ReligiousDenominationDTO()
        {
            Code = religiousDenomination.Code,
            Name = religiousDenomination.Name
        };
    }
}