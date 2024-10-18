using ASP.Core;
using ASP.Core.LocalAuthorities;
using ASP.Core.Results;
using ASP.Core.Utilities;
using ASP.Infrastructure.LocalAuthorities.DAO;
using ASP.Infrastructure.LocalAuthorities.DAO.Mapper;

namespace ASP.Infrastructure.LocalAuthorities;

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
}