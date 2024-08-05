namespace ASP.Infrastructure.Establishments.DAO.Mapper;

public static class LocalAuthorityMapper
{
    public static Core.Establishments.LocalAuthority? MapToDomainEntityLocalAuthority(this LocalAuthorityDAO? localAuthority)
    {
        if (localAuthority == null) return null;  // Return null directly instead of an empty object
        return new Core.Establishments.LocalAuthority()
        {
            Code = localAuthority.Code,
            Name = localAuthority.Name
        };
    }

    public static LocalAuthorityDAO? MapToLocalAuthorityDAO(this Core.Establishments.LocalAuthority? localAuthority)
    {
        if (localAuthority == null) return null;  // Return null directly instead of an empty object
        return new LocalAuthorityDAO(localAuthority.Code, localAuthority.Name);
    }
}