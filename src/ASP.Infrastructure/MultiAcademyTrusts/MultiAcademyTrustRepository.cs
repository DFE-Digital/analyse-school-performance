using ASP.Core;
using ASP.Core.MultiAcademyTrusts;
using ASP.Core.Results;
using ASP.Infrastructure.MultiAcademyTrusts.DAO;
using ASP.Infrastructure.MultiAcademyTrusts.DAO.Mapper;

namespace ASP.Infrastructure.LocalAuthorities;

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
        return _documentDB.GetAsync<MultiAcademyTrustDAO>(ContainerKey, id, id)
            .MapError(e => e is NotFoundError
                ? Error.NotFound($@"Could not find Multi-Academy Trust with id ""{id}"".")
                : e)
            .Map(dto => dto.MapToDomainEntityLocalAuthority());
    }
}