using ASP.Domain.Establishments;

namespace ASP.Domain.Repositories.Establishments.DAO.Mapper
{
    public static class MultiAcademyTrustMapper
    {
        public static MultiAcademyTrust? MapToDomainEntityMultiAcademyTrust(this MultiAcademyTrustDAO? multiAcademyTrustDAO)
        {
            if (multiAcademyTrustDAO is null) return null;

            return new MultiAcademyTrust(
                multiAcademyTrustDAO.Uid,
                multiAcademyTrustDAO.Name
            );
        }

        public static MultiAcademyTrustDAO? MapToMultiAcademyTrustDOA(this MultiAcademyTrust multiAcademyTrust)
        {
            if (multiAcademyTrust is null) return null;

            return new MultiAcademyTrustDAO(
                multiAcademyTrust.Id,
                multiAcademyTrust.Name
            );
        }
    }
}
