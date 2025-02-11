using ASP.Domain.Establishments;

namespace ASP.Domain.Repositories.Establishments.DAO.Mapper
{
    public static class DioceseMapper
    {
        public static Diocese? MapToDomainEntityDiocese(this DioceseDAO? dioceseDAO)
        {
            if (dioceseDAO is null) return null;

            return new Diocese(
                dioceseDAO.Code,
                dioceseDAO.Name
            );
        }

        public static DioceseDAO? MapToDioceseDOA(this Diocese diocese)
        {
            if (diocese is null) return null;

            return new DioceseDAO(
                diocese.Id,
                diocese.Name
            );
        }
    }
}
