using ASP.Core.Results;

namespace ASP.Domain.LocalAuthorities.UseCases.GetLocalAuthority;

public class GetLocalAuthority : IGetLocalAuthority
{
    private readonly ILocalAuthorityRepository _repository;

    public GetLocalAuthority(ILocalAuthorityRepository localAuthorityRepository)
    {
        _repository = localAuthorityRepository ??
                      throw new ArgumentNullException(nameof(localAuthorityRepository));
    }

    public Task<Result<LocalAuthority>> HandleRequest(GetLocalAuthorityRequest request)
    {
        return
            from la in _repository.GetLocalAuthority(request.Code)
            select la;
    }
}