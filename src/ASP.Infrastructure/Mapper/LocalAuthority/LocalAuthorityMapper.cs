using ASP.Infrastructure.DAO.LocalAuthority;

namespace ASP.Infrastructure.Mapper.LocalAuthority;

public static class LocalAuthorityMapper
{
    public static Core.LocalAuthority.LocalAuthority MapToDomainEntityLocalAuthority(this LocalAuthorityDAO localAuthority)
    {
        return new Core.LocalAuthority.LocalAuthority(localAuthority.Code, localAuthority.Name);
    }
    
    public static LocalAuthorityDAO? MapToLocalAuthorityDAO(this Core.LocalAuthority.LocalAuthority localAuthority)
    {
        return new LocalAuthorityDAO(localAuthority.Code, localAuthority.Name);
    }
}