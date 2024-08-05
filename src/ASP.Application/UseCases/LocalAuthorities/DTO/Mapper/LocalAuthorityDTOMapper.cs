using ASP.Core.LocalAuthorities;

namespace ASP.Application.UseCases.LocalAuthorities.DTO.Mapper;

public static class LocalAuthorityDTOMapper
{
    public static LocalAuthorityDTO MapToLocalAuthorityDTO(this LocalAuthority localAuthority)
    {
        return new LocalAuthorityDTO(localAuthority.Code, localAuthority.Name);
    }
}