using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Infrastructure.DocumentDatabase;
using ASP.Core.Optionality;
using ASP.Domain.Schools;
using ASP.Domain.Schools.LinkedSchools;
using ASP.Domain.Schools.Details;
using ASP.Domain.Schools.Search;
using ASP.Domain.Schools.Access;

namespace ASP.Domain.Repositories.Schools
{
    public class SchoolRepository : ISchoolRepository
    {
        private const string ContainerKey = "establishments";
        private readonly IDocumentDatabase _documentDB;

        public SchoolRepository(IDocumentDatabase documentDB)
        {
            _documentDB = documentDB ??
                throw new ArgumentNullException(nameof(documentDB));
        }

        public Task<Result<School>> Get(
            SchoolUrn urn,
            CancellationToken cancellationToken = default)
        {
            return
                from dao in GetEstablishment(urn, cancellationToken)
                from result in FromDao(dao)
                select result;
        }

        public Task<Result<SchoolWithEstablishmentDetails>> GetWithEstablishmentDetails(
            SchoolUrn urn,
            CancellationToken cancellationToken = default)
        {
            return
                from dao in GetEstablishment(urn, cancellationToken)
                from result in FromDaoWithEstablishmentDetails(dao)
                select result;
        }

        public Task<Result<SchoolWithLinks>> GetWithLinkedSchools(
            SchoolUrn urn,
            CancellationToken cancellationToken = default)
        {
            return
                from dao in GetEstablishment(urn, cancellationToken)
                from linkedSchools in GetLinkedSchools(dao, cancellationToken)
                from result in FromDaoWithLinkedSchools(dao, linkedSchools)
                select result;
        }

        public Task<Result<ResultsPage<School>>> GetAll(
            Optional<SchoolAccessScope> scope,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            return
                from results in QueryEstablishmentsPaged(
                    q => q.VisibleAndNotDeleted()
                          .InScope(scope)
                          .OrderedByName(),
                    page,
                    resultsPerPage,
                    cancellationToken)
                    .ErrorIf(q => q.TotalResults == 0, Error.NotFound("There were no schools within the given scope."))
                from schools in results.Results.Select(FromDao).Combine()
                select new ResultsPage<School>(results.Page, results.ResultsPerPage, results.TotalResults, schools);
        }

        public Task<Result<ResultsPage<School>>> SearchByCriteria(
            ISearchCriteria criteria,
            Optional<SchoolAccessScope> scope,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            return
                from results in QueryEstablishmentsPaged(
                    q => q.VisibleAndNotDeleted()
                          .InScope(scope)
                          .MatchingSearchCriteria(criteria)
                          .OrderedByName(),
                    page,
                    resultsPerPage,
                    cancellationToken)
                    .ErrorIf(q => q.TotalResults == 0, Error.NotFound($@"There were no matches for ""{criteria.RawValue}"" within the given scope."))
                from schools in results.Results.Select(FromDao).Combine()
                select new ResultsPage<School>(results.Page, results.ResultsPerPage, results.TotalResults, schools);
        }

        public Task<Result<List<School>>> SearchSuggestionsByCriteria(
            ISearchCriteria criteria,
            Optional<SchoolAccessScope> scope,
            int maxSuggestions,
            CancellationToken cancellationToken = default)
        {
            return
                from results in QueryEstablishments(
                    q => q.VisibleAndNotDeleted()
                          .InScope(scope)
                          .MatchingSearchCriteria(criteria)
                          .OrderedBySearchCriteria(criteria)
                          .Take(maxSuggestions),
                    cancellationToken)
                    .ErrorIf(r => !r.Any(), Error.NotFound($@"There were no matches for ""{criteria.RawValue}"" within the given scope."))
                from schools in results.Select(FromDao).ToList().Combine()
                select schools;
        }

        private async Task<Result<List<LinkedSchoolsLink>>> GetLinkedSchools(
            EstablishmentDao dao,
            CancellationToken cancellationToken = default)
        {
            var links = dao.Links ?? [];

            if (!links.Any())
            {
                return Result.Success(new List<LinkedSchoolsLink>());
            }

            var distinctUrns = links.Select(l => l.LinkedUrn).Distinct();

            return
                from daos in await QueryEstablishments(
                    q => q.VisibleAndNotDeleted()
                          .WithUrnIn(distinctUrns)
                          .OrderedByName(),
                    cancellationToken)
                let linkedEstablishments = daos.ToDictionary(dao => dao.Urn, dao => dao)
                from responses in CreateLinks(dao, linkedEstablishments)
                select responses;
        }

        private Task<Result<EstablishmentDao>> GetEstablishment(
            SchoolUrn urn,
            CancellationToken cancellationToken = default)
        {
            return
                from dao in _documentDB.GetAsync<EstablishmentDao>(ContainerKey, urn.Value, urn.Value, cancellationToken)
                    .MapErrorIf(e => e is NotFoundError, Error.NotFound($@"Could not find school with URN ""{urn.Value}""."))
                    .ErrorIf(estab => estab.IsDeleted, Error.NotFound($@"Could not find school with URN ""{urn.Value}""."))
                    .ErrorIf(estab => !estab.IsVisible, Error.NotFound($@"Could not find school with URN ""{urn.Value}""."))
                select dao;
        }

        private async Task<Result<IEnumerable<EstablishmentDao>>> QueryEstablishments(
            Func<IQueryable<EstablishmentDao>, IQueryable<EstablishmentDao>> query,
            CancellationToken cancellationToken = default)
        {
            return
                from daos in await _documentDB.QueryAsync(ContainerKey, query, cancellationToken)
                select daos;
        }

        private async Task<Result<ResultsPage<EstablishmentDao>>> QueryEstablishmentsPaged(
            Func<IQueryable<EstablishmentDao>, IQueryable<EstablishmentDao>> query,
            int page,
            int resultsPerPage,
            CancellationToken cancellationToken = default)
        {
            return
                from daos in await _documentDB.QueryPagedAsync(ContainerKey, query, page, resultsPerPage, cancellationToken)
                select daos;
        }

        private Result<List<LinkedSchoolsLink>> CreateLinks(
            EstablishmentDao dao,
            Dictionary<string, EstablishmentDao> linkedEstablishments)
        {
            var links = dao.Links ?? [];

            var validLinks = links
                .Where(l => linkedEstablishments.ContainsKey(l.LinkedUrn))
                .ToList();

            return validLinks
                .GroupBy(l => new { l.EstablishedDate, TypeCode = l.LinkType?.Code })
                .OrderBy(g => g.Key.EstablishedDate)
                .Select(g =>
                {
                    var establishments = g
                        .Select(l => linkedEstablishments[l.LinkedUrn])
                        .OrderBy(e => e.Urn)
                        .ToList();

                    return
                        from schools in establishments.Select(FromDao).ToList().Combine()
                        select new LinkedSchoolsLink(
                            dao.Name,
                            dao.OpenDate,
                            dao.CloseDate,
                            g.Key.EstablishedDate,
                            MapNullable(g.First().LinkType, FromDaoAsLinkType),
                            schools);
                })
                .ToList()
                .Combine();
        }

        private Result<School> FromDao(EstablishmentDao dao)
        {
            return
                from urn in SchoolUrn.Parse(dao.Urn)
                from laestab in LAEstabCode.Parse(dao.Laestab)
                    .Map(l => (LAEstabCode?)l).DefaultIfError(null)
                select new School(
                    urn,
                    laestab,
                    dao.Name,
                    new EducationPhase(
                        dao.IsPrimary,
                        dao.IsSecondary,
                        dao.IsPost16),
                    MapNullable(dao.Address, FromDao),
                    MapNullable(dao.OpenDate, d => d),
                    MapNullable(dao.CloseDate, d => d),
                    MapNullable(dao.LocalAuthority, FromDaoAsLocalAuthority),
                    MapNullable(dao.MultiAcademyTrust, FromDao),
                    MapNullable(dao.Diocese, FromDaoAsDiocese)
                );
        }

        private Result<SchoolWithEstablishmentDetails> FromDaoWithEstablishmentDetails(EstablishmentDao dao)
        {
            return
                from urn in SchoolUrn.Parse(dao.Urn)
                from laestab in LAEstabCode.Parse(dao.Laestab)
                    .Map(l => (LAEstabCode?)l).DefaultIfError(null)
                select new SchoolWithEstablishmentDetails(
                    urn,
                    laestab,
                    dao.Name,
                    new EducationPhase(
                        dao.IsPrimary,
                        dao.IsSecondary,
                        dao.IsPost16),
                    MapNullable(dao.Address, FromDao),
                    MapNullable(dao.OpenDate, d => d),
                    MapNullable(dao.CloseDate, d => d),
                    MapNullable(dao.LocalAuthority, FromDaoAsLocalAuthority),
                    MapNullable(dao.MultiAcademyTrust, FromDao),
                    MapNullable(dao.Diocese, FromDaoAsDiocese),
                    new EstablishmentDetails(
                        dao.EstablishmentType?.Name ?? "Data not available",
                        dao.Gender?.Name ?? "Data not available",
                        dao.HeadTeacher != null && (!string.IsNullOrWhiteSpace(dao.HeadTeacher.Title) || !string.IsNullOrWhiteSpace(dao.HeadTeacher.FirstName) || !string.IsNullOrWhiteSpace(dao.HeadTeacher.LastName))
                            ? $"{dao.HeadTeacher.Title} {dao.HeadTeacher.FirstName} {dao.HeadTeacher.LastName}" 
                            : "Data not available",
                        dao.AgeRange != null && (!string.IsNullOrWhiteSpace(dao.AgeRange.Low) || !string.IsNullOrWhiteSpace(dao.AgeRange.Low))
                            ? $"{dao.AgeRange.Low} to {dao.AgeRange.High}" 
                            : "Data not available",
                        dao.ReligiousDenomination?.Name ?? "Data not available",
                        dao.AdmissionsPolicy?.Name ?? "Data not available",
                        dao.ResourcedProvisionType?.Name ?? "Data not available",
                        dao.NoOfPupils?.ToString() ?? "Data not available"));
        }

        private Result<SchoolWithLinks> FromDaoWithLinkedSchools(EstablishmentDao dao, List<LinkedSchoolsLink> links)
        {
            return
                from urn in SchoolUrn.Parse(dao.Urn)
                from laestab in LAEstabCode.Parse(dao.Laestab)
                    .Map(l => (LAEstabCode?)l).DefaultIfError(null)
                select new SchoolWithLinks(
                    urn,
                    laestab,
                    dao.Name,
                    new EducationPhase(
                        dao.IsPrimary,
                        dao.IsSecondary,
                        dao.IsPost16),
                    MapNullable(dao.Address, FromDao),
                    MapNullable(dao.OpenDate, d => d),
                    MapNullable(dao.CloseDate, d => d),
                    MapNullable(dao.LocalAuthority, FromDaoAsLocalAuthority),
                    MapNullable(dao.MultiAcademyTrust, FromDao),
                    MapNullable(dao.Diocese, FromDaoAsDiocese),
                    links);
        }

        private U? MapNullable<T, U>(T? t, Func<T, U> mapFunction)
            where T : notnull
            where U : notnull
            => t is null ? default : mapFunction(t);

        private U? MapNullable<T, U>(T? t, Func<T, U> mapFunction)
            where T : struct
            where U : struct
            => t is null ? default : mapFunction(t.Value);

        private LocalAuthority FromDaoAsLocalAuthority(LookupValueDao dao)
            => new LocalAuthority(dao.Code, dao.Name);

        private MultiAcademyTrust FromDao(MultiAcademyTrustDao dao)
            => new MultiAcademyTrust(dao.Uid, dao.Name);

        private Diocese FromDaoAsDiocese(LookupValueDao dao)
            => new Diocese(dao.Code, dao.Name);

        private Address FromDao(AddressDao dao)
            => new Address(dao.Street, dao.Town, dao.PostCode);

        private LinkType FromDaoAsLinkType(LookupValueDao dao)
            => new LinkType(dao.Code, dao.Name);

        private LookupValue FromDaoAsLookupValue(LookupValueDao dao)
            => new LookupValue(dao.Code, dao.Name);

        private AgeRange FromDao(AgeRangeDao dao)
            => new AgeRange(dao.Low, dao.High);

        private HeadTeacher FromDao(HeadTeacherDao dao)
            => new HeadTeacher(dao.Title, dao.FirstName, dao.LastName, dao.PreferredJobTitle);
    }
}