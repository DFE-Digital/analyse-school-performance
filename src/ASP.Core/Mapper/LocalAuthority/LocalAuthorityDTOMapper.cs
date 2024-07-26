using ASP.Core.DTO.LocalAuthority;

namespace ASP.Core.Mapper.LocalAuthority;

public static class LocalAuthorityDTOMapper
{
    public static LocalAuthorityDTO MapToLocalAuthorityDTO(this Core.LocalAuthority.LocalAuthority localAuthority)
    {
        return new LocalAuthorityDTO(localAuthority.Code, localAuthority.Name);
    }
}