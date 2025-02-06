using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain.LocalAuthorities;
using ASP.Infrastructure.DocumentDatabase;
using ASP.Domain.Repositories.LocalAuthorities.DAO;
using ASP.Domain.Repositories.LocalAuthorities.DAO.Mapper;

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

    public Task<Result<LocalAuthority>> GetLocalAuthority(string code)
    {
        return
            from dao in _documentDB.GetAsync<LocalAuthorityDAO>(ContainerKey, code, code)
                .MapError(e => e is NotFoundError
                    ? Error.NotFound($@"Could not find Local Authority with code ""{code}"".")
                    : e)
            select dao.MapToDomainEntityLocalAuthority();
    }

    public async Task<Result<ResultsPage<LocalAuthority>>> GetAllLocalAuthorities(int page,
        int resultsPerPage, CancellationToken cancellationToken = default)
    {
        return
            from results in await _documentDB.QueryPagedAsync<LocalAuthorityDAO>(
                    ContainerKey,
                    q => q.OrderBy(x => x.Name),
                    page,
                    resultsPerPage,
                    cancellationToken)
                .ErrorIf(q => q.TotalResults == 0, Error.NotFound($@"there were no Local Authorities."))
            select new ResultsPage<LocalAuthority>(
                page,
                resultsPerPage,
                results.TotalResults,
                results.Map(r => r.MapToDomainEntityLocalAuthority()));
    }

    public Task<Result<List<LocalAuthority>>> LocalAuthoritySearchSuggestionsByLaName(
        string searchTerm, int maxSuggestions, CancellationToken cancellationToken = default)
    {
        return
            from results in _documentDB.QueryAsync<LocalAuthorityDAO>(
                    ContainerKey,
                    q => q.Where(x =>
                            x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                        ).OrderBy(x => x.Name)
                        .Take(maxSuggestions),
                    cancellationToken)
                .ErrorIf(r => !r.Any(), Error.NotFound($@"there were no matches for ""{searchTerm}""."))
            select results.MapToDomainEntityLocalAuthority();
    }

    public Task<Result<List<LocalAuthority>>> LocalAuthoritySearchSuggestionsByLaCode(
        string searchTerm, int maxSuggestions, CancellationToken cancellationToken = default)
    {
        return
            from results in _documentDB.QueryAsync<LocalAuthorityDAO>(
                    ContainerKey,
                    q => q.Where(x =>
                            x.Code.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                        ).OrderBy(x => x.Code)
                        .Take(maxSuggestions),
                    cancellationToken)
                .ErrorIf(r => !r.Any(), Error.NotFound($@"there were no matches for ""{searchTerm}""."))
            select results.MapToDomainEntityLocalAuthority();
    }

    public Task<Result<SearchResultsPage<LocalAuthority>>> LocalAuthoritySearchByLaName(
        string searchTerm, int page, int resultsPerPage,
        CancellationToken cancellationToken = default)
    {
        return
            from results in _documentDB.QueryPagedAsync<LocalAuthorityDAO>(
                    ContainerKey,
                    q => q.Where(x =>
                        x.Name.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)
                    ).OrderBy(x => x.Name),
                    page,
                    resultsPerPage,
                    cancellationToken)
                .ErrorIf(r => r.TotalResults == 0, Error.NotFound($@"there were no matches for ""{searchTerm}""."))
            select new SearchResultsPage<LocalAuthority>(searchTerm,
                results.Map(r => r.MapToDomainEntityLocalAuthority()));
    }
}