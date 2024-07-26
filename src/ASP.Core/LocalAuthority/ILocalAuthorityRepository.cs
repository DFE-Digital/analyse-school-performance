using ASP.Core.Results;

namespace ASP.Core.LocalAuthority;

public interface ILocalAuthorityRepository
{
    Task<Result<LocalAuthority>> GetLocalAuthority(string code);
}