using ASP.Core.Results;

namespace ASP.Domain.MultiAcademyTrusts;

public interface IMultiAcademyTrustRepository
{
    Task<Result<MultiAcademyTrust>> GetMultiAcademyTrust(string id);
}