using ASP.Core.Results;
using ASP.Domain.MultiAcademyTrusts;
using ASP.Infrastructure.DocumentDatabase;
using ASP.Infrastructure.Repositories.MultiAcademyTrusts.DAO;
using ASP.Infrastructure.Repositories.MultiAcademyTrusts.DAO.Mapper;

namespace ASP.Infrastructure.Repositories.MultiAcademyTrusts;

public class MultiAcademyTrustRepository : IMultiAcademyTrustRepository
{
    private const string ContainerKey = "multi-academy-trusts";
    private readonly IDocumentDatabase _documentDB;

    public MultiAcademyTrustRepository(IDocumentDatabase documentDB)
    {
        _documentDB = documentDB ??
                      throw new ArgumentNullException(nameof(documentDB));
    }

    public Task<Result<MultiAcademyTrust>> GetMultiAcademyTrust(string id)
    {
        return
            from dao in _documentDB.GetAsync<MultiAcademyTrustDAO>(ContainerKey, id, id)
                .MapError(e => e is NotFoundError
                    ? Error.NotFound($@"Could not find Multi-Academy Trust with id ""{id}"".")
                    : e)
            select dao.MapToDomainEntityLocalAuthority();
    }
}