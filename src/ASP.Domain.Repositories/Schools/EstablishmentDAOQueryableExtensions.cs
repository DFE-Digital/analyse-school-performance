using ASP.Core.Optionality;
using ASP.Domain.Schools.Access;
using ASP.Domain.Schools.Search;

namespace ASP.Domain.Repositories.Schools
{
    public static class EstablishmentDaoQueryableExtensions
    {
        public static IQueryable<EstablishmentDao> VisibleAndNotDeleted(this IQueryable<EstablishmentDao> query)
        {
            return query.Where(x => !x.IsDeleted && x.IsVisible);
        }

        public static IQueryable<EstablishmentDao> InScope(this IQueryable<EstablishmentDao> query, Optional<SchoolAccessScope> scope)
        {
            return scope.Match(
                s => s.ScopeType switch {
                    SchoolAccessScopeType.LA => query.Where(x => x.LocalAuthority != null
                        && x.LocalAuthority.Code.ToString() == s.ScopeIdentifier),

                    SchoolAccessScopeType.MAT => query.Where(x => x.MultiAcademyTrust != null
                        && x.MultiAcademyTrust.Uid.ToString() == s.ScopeIdentifier),

                    SchoolAccessScopeType.Diocese => query.Where(x => x.Diocese != null
                        && x.Diocese.Name == s.ScopeIdentifier),

                    _ => query
                },
                () => query);
        }

        public static IQueryable<EstablishmentDao> MatchingSearchCriteria(this IQueryable<EstablishmentDao> query, ISearchCriteria criteria)
        {
            return criteria switch {
                FullSchoolUrnSearchCriteria c => query.Where(x =>
                    x.Urn.Equals(c.RawValue, StringComparison.CurrentCultureIgnoreCase)),

                PartialSchoolUrnSearchCriteria c => query.Where(x =>
                    x.Urn.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase)),

                FullLACodeOrEstabCodeSearchCriteria c => query.Where(x =>
                    x.Laestab != null &&  // Ensure Laestab is not null before calling Contains
                        x.Laestab.Contains(c.SearchTerm, StringComparison.CurrentCultureIgnoreCase)),

                PartialLAEstabCodeSearchCriteria c => query.Where(x =>
                    x.Laestab != null && ( // Ensure Laestab is not null before calling Contains
                        x.Laestab.Contains(c.SearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Laestab.Replace("/", "").Contains(c.SearchTerm, StringComparison.CurrentCultureIgnoreCase))),

                NameOrAddressSearchCriteria c => query.Where(x =>
                    x.Name.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Address != null && ( // Ensure Address is not null before calling Contains
                        x.Address.Street.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Address.Town.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Address.PostCode.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase))),

                PartialLaEstabCodeOrNameOrAddressSearchCriteria c => query.Where(x =>
                    x.Name.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase) ||
                    x.Address != null && ( // Ensure Address is not null before calling Contains
                        x.Address.Street.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Address.Town.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Address.PostCode.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase)) ||
                    x.Laestab != null && ( // Ensure Laestab is not null before calling Contains
                        x.Laestab.Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Laestab.Replace("/", "").Contains(c.RawValue, StringComparison.CurrentCultureIgnoreCase))),

                _ => throw new NotImplementedException($"No query capability implemented for {criteria.GetType().Name}.")
            };
        }

        public static IQueryable<EstablishmentDao> WithUrnIn(this IQueryable<EstablishmentDao> query, IEnumerable<string> urns)
        {
            return query.Where(x => urns.Contains(x.Urn));
        }

        public static IQueryable<EstablishmentDao> OrderedByName(this IQueryable<EstablishmentDao> query)
        {
            return query.OrderBy(x => x.Name);
        }

        public static IQueryable<EstablishmentDao> OrderedBySearchCriteria(this IQueryable<EstablishmentDao> query, ISearchCriteria criteria)
        {
            return criteria switch {
                FullSchoolUrnSearchCriteria c => query.OrderBy(x => x.Urn),

                PartialSchoolUrnSearchCriteria c => query.OrderBy(x => x.Urn),

                FullLACodeOrEstabCodeSearchCriteria c => query.OrderBy(x => x.Laestab),

                PartialLAEstabCodeSearchCriteria c => query.OrderBy(x => x.Laestab),

                NameOrAddressSearchCriteria c => query.OrderBy(x => x.Name),

                PartialLaEstabCodeOrNameOrAddressSearchCriteria c => query.OrderBy(x => x.Name),

                _ => throw new NotImplementedException($"No query capability implemented for {criteria.GetType().Name}.")
            };
        }
    }
}