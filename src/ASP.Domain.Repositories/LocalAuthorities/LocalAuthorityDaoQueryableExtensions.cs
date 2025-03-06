using ASP.Domain.LocalAuthorities;

namespace ASP.Domain.Repositories.LocalAuthorities
{
    public static class LocalAuthorityDaoQueryableExtensions
    {
        public static IQueryable<LocalAuthorityDao> MatchingSearchCriteria(this IQueryable<LocalAuthorityDao> query, ILocalAuthoritySearchCriteria criteria)
        {
            return criteria switch {
                NameSearchCriteria c => query.Where(x =>
                    x.Name.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase)),

                PartialCodeSearchCriteria c => query.Where(x =>
                    x.Code != null && ( // Ensure Code is not null before calling Contains
                        x.Code.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase))),

                _ => throw new NotImplementedException($"No query capability implemented for {criteria.GetType().Name}.")
            };
        }

        public static IQueryable<LocalAuthorityDao> OrderedByName(this IQueryable<LocalAuthorityDao> query)
        {
            return query.OrderBy(x => x.Name);
        }

        public static IQueryable<LocalAuthorityDao> OrderedBySearchCriteria(this IQueryable<LocalAuthorityDao> query, ILocalAuthoritySearchCriteria criteria)
        {
            return criteria switch {
                NameSearchCriteria c => query.OrderBy(x => x.Name),

                PartialCodeSearchCriteria c => query.OrderBy(x => x.Code),

                _ => throw new NotImplementedException($"No query capability implemented for {criteria.GetType().Name}.")
            };
        }
    }
}