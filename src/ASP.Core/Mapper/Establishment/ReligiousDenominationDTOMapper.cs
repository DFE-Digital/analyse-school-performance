using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

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