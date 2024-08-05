namespace ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;

public class GetLocalAuthorityRequest
{
    public string Code { get; set; }

    public GetLocalAuthorityRequest(string code)
    {
        Code = code;
    }
}