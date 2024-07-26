namespace ASP.Infrastructure.Mapper.Establishment;

public static class LocalAuthorityMapper
{
    public static Core.Establishments.LocalAuthority? MapToDomainEntityLocalAuthority(this DAO.Establishment.LocalAuthority? localAuthority)
    {
        if (localAuthority == null) return null;  // Return null directly instead of an empty object
        return new Core.Establishments.LocalAuthority()
        {
            Code = localAuthority.Code,
            Name = localAuthority.Name
        };
    }
    
    public static DAO.Establishment.LocalAuthority? MapToLocalAuthorityDAO(this Core.Establishments.LocalAuthority? localAuthority)
    {
        if (localAuthority == null) return null;  // Return null directly instead of an empty object
        return new DAO.Establishment.LocalAuthority(localAuthority.Code, localAuthority.Name);
    }
}