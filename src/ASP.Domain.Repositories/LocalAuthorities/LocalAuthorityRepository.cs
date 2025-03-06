using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.LocalAuthorities;
using ASP.Infrastructure.DocumentDatabase;

namespace ASP.Domain.Repositories.LocalAuthorities;

public class LocalAuthorityRepository : ILocalAuthorityRepository
{
    private const string ContainerKey = "local-authorities";
    private readonly IDocumentDatabase _documentDB;

    public LocalAuthorityRepository(IDocumentDatabase documentDB)
    {
        _documentDB = documentDB ??
                      throw new ArgumentNullException(nameof(documentDB));
    }

    public Task<Result<LocalAuthority>> Get(LACode code)
    {
        return
            from dao in _documentDB.GetAsync<LocalAuthorityDao>(ContainerKey, code.Value, code.Value)
                .MapErrorIf(e => e is NotFoundError, Error.NotFound($@"Could not find Local Authority with code ""{code.Value}""."))
            from la in FromDao(dao)
            select la;
    }

    public async Task<Result<ResultsPage<LocalAuthority>>> GetAll(
        int page,
        int resultsPerPage,
        CancellationToken cancellationToken = default)
    {
        return
            from results in await _documentDB.QueryPagedAsync<LocalAuthorityDao>(
                    ContainerKey,
                    q => q.OrderBy(x => x.Name),
                    page,
                    resultsPerPage,
                    cancellationToken)
                .ErrorIf(q => q.TotalResults == 0, Error.NotFound($@"there were no Local Authorities."))
            from las in results.Results.Select(FromDao).ToList().Combine()
            select new ResultsPage<LocalAuthority>(page, resultsPerPage, results.TotalResults, las);
    }

    public Task<Result<ResultsPage<LocalAuthority>>> Search(
        ILocalAuthoritySearchCriteria criteria,
        int page,
        int resultsPerPage,
        CancellationToken cancellationToken = default)
    {
        return
            from results in _documentDB.QueryPagedAsync<LocalAuthorityDao>(
                    ContainerKey,
                    q => q.MatchingSearchCriteria(criteria)
                          .OrderedByName(),
                    page,
                    resultsPerPage,
                    cancellationToken)
                .ErrorIf(r => r.TotalResults == 0, Error.NotFound($@"there were no matches for ""{criteria.RawValue}""."))
            from las in results.Results.Select(FromDao).ToList().Combine()
            select new ResultsPage<LocalAuthority>(page, resultsPerPage, results.TotalResults, las);
    }

    public Task<Result<List<LocalAuthority>>> SearchSuggestions(
        ILocalAuthoritySearchCriteria criteria,
        int maxSuggestions,
        CancellationToken cancellationToken = default)
    {
        return
            from results in _documentDB.QueryAsync<LocalAuthorityDao>(
                    ContainerKey,
                    q => q.MatchingSearchCriteria(criteria)
                          .OrderedBySearchCriteria(criteria)
                          .Take(maxSuggestions),
                    cancellationToken)
                .ErrorIf(r => !r.Any(), Error.NotFound($@"there were no matches for ""{criteria.RawValue}""."))
            from las in results.Select(FromDao).ToList().Combine()
            select las;
    }

    private Result<LocalAuthority> FromDao(LocalAuthorityDao dao)
    {
        return 
            from code in LACode.Parse(dao.Code)
            select new LocalAuthority(code, dao.Name);
    }
}