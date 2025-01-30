namespace ASP.Domain.LocalAuthorities.UseCases.DTO.Mapper;

public static class LocalAuthorityDTOMapper
{
    public static LocalAuthorityDTO MapToLocalAuthorityDTO(this LocalAuthority localAuthority)
    {
        return new LocalAuthorityDTO(localAuthority.Code, localAuthority.Name);
    }
    
    public static List<LocalAuthorityDTO> MapToLocalAuthorityDTO(
        this IEnumerable<LocalAuthority> localAuthorities)
    {
        return localAuthorities.Select(MapToLocalAuthorityDTO).ToList();
    }
}