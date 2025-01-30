using ASP.Domain.MultiAcademyTrusts;

namespace ASP.Infrastructure.Repositories.MultiAcademyTrusts.DAO.Mapper;

public static class MultiAcademyTrustMapper
{
    public static MultiAcademyTrust MapToDomainEntityLocalAuthority(this MultiAcademyTrustDAO multiAcademyTrust)
    {
        return new MultiAcademyTrust(multiAcademyTrust.Id, multiAcademyTrust.Name);
    }

    public static MultiAcademyTrustDAO? MapToLocalAuthorityDAO(this MultiAcademyTrust multiAcademyTrust)
    {
        return new MultiAcademyTrustDAO(multiAcademyTrust.Id, multiAcademyTrust.Name);
    }
}