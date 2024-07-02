using ASP.Infrastructure.DAO.Establishment;

namespace ASP.Infrastructure.Mapper.Establishment;

public static class LocalAuthorityMapper
{
    public static Core.Establishments.LocalAuthority? MapToDomainEntityLocalAuthority(this LocalAuthority? localAuthority)
    {
        if (localAuthority == null) return null;  // Return null directly instead of an empty object
        return new Core.Establishments.LocalAuthority()
        {
            Code = localAuthority.Code,
            Name = localAuthority.Name
        };
    }
    
    public static LocalAuthority? MapToLocalAuthorityDAO(this Core.Establishments.LocalAuthority? localAuthority)
    {
        if (localAuthority == null) return null;  // Return null directly instead of an empty object
        return new LocalAuthority(localAuthority.Code, localAuthority.Name);
    }
}