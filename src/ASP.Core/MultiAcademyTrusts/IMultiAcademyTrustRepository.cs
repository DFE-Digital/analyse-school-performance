using ASP.Core.Results;

namespace ASP.Core.MultiAcademyTrusts;

public interface IMultiAcademyTrustRepository
{
    Task<Result<MultiAcademyTrust>> GetMultiAcademyTrust(string id);
}