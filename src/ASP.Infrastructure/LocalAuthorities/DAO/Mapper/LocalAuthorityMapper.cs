using ASP.Core.LocalAuthorities;

namespace ASP.Infrastructure.LocalAuthorities.DAO.Mapper;

public static class LocalAuthorityMapper
{
    public static LocalAuthority MapToDomainEntityLocalAuthority(this LocalAuthorityDAO localAuthority)
    {
        return new LocalAuthority(localAuthority.Code, localAuthority.Name);
    }

    public static LocalAuthorityDAO? MapToLocalAuthorityDAO(this LocalAuthority localAuthority)
    {
        return new LocalAuthorityDAO(localAuthority.Code, localAuthority.Name);
    }
    
    public static List<LocalAuthority> MapToDomainEntityLocalAuthority(
        this IEnumerable<LocalAuthorityDAO> localAuthoritiesDao)
    {
        return localAuthoritiesDao.Select(MapToDomainEntityLocalAuthority).ToList();
    }
}