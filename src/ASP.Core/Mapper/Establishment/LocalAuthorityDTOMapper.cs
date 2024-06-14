using ASP.Core.DTO.Establishment;
using ASP.Core.Establishments;

namespace ASP.Core.Mapper.Establishment;

public static class LocalAuthorityDTOMapper
{
    public static LocalAuthorityDTO MapToLocalAuthorityDTO(this LocalAuthority? localAuthority)
    {
        if (localAuthority == null) return new LocalAuthorityDTO();
        return new LocalAuthorityDTO()
        {
           Code = localAuthority.Code,
           Name = localAuthority.Name
        };
    }
}