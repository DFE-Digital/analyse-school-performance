namespace ASP.Application.UseCases.Establishments.DTO.Mapper;

public static class LocalAuthorityDTOMapper
{
    public static LocalAuthorityDTO? MapToLocalAuthorityDTO(this Core.Establishments.LocalAuthority? localAuthority)
    {
        if (localAuthority == null) return null;  // Return null directly instead of an empty object
        return new LocalAuthorityDTO()
        {
            Code = localAuthority.Code,
            Name = localAuthority.Name
        };
    }
}