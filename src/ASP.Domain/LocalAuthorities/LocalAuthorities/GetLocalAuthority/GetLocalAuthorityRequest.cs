namespace ASP.Domain.LocalAuthorities.UseCases.GetLocalAuthority;

public class GetLocalAuthorityRequest
{
    public string Code { get; set; }

    public GetLocalAuthorityRequest(string code)
    {
        Code = code;
    }
}