using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class ReligiousDenominationDTOMapper
{
    public static ReligiousDenominationDTO MapToReligiousDenominationDTO(this ReligiousDenomination? religiousDenomination)
    {
        if (religiousDenomination == null) return new ReligiousDenominationDTO();
        return new ReligiousDenominationDTO()
        {
           Code = religiousDenomination.Code,
           Name = religiousDenomination.Name
        };
    }
}