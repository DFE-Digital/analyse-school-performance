using ASP.Core.Results;

namespace ASP.Core.LocalAuthorities;

public interface ILocalAuthorityRepository
{
    Task<Result<LocalAuthority>> GetLocalAuthority(string code);
}