namespace ASP.Domain.Repositories.Establishments.DAO.Mapper;

public static class LocalAuthorityMapper
{
    public static Domain.Establishments.LocalAuthority? MapToDomainEntityLocalAuthority(this LocalAuthorityDAO? localAuthority)
    {
        if (localAuthority == null) return null;  // Return null directly instead of an empty object

        return new Domain.Establishments.LocalAuthority(
            localAuthority.Code,
            localAuthority.Name
        );
    }

    public static LocalAuthorityDAO? MapToLocalAuthorityDAO(this Domain.Establishments.LocalAuthority? localAuthority)
    {
        if (localAuthority == null) return null;  // Return null directly instead of an empty object

        return new LocalAuthorityDAO(
            localAuthority.Code,
            localAuthority.Name
        );
    }
}