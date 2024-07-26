using ASP.Core;
using ASP.Core.LocalAuthority;
using ASP.Core.Results;
using ASP.Infrastructure.Mapper.LocalAuthority;

namespace ASP.Infrastructure.Repositories;

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
        return _documentDB.GetAsync<DAO.LocalAuthority.LocalAuthorityDAO>(ContainerKey, code, code)
            .MapError(e => e is NotFoundError
                ? Error.NotFound($@"Could not find local authority with code ""{code}"".")
                : e)
            .Map(dto => dto.MapToDomainEntityLocalAuthority());
    }
}