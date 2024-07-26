namespace ASP.Application.UseCases.LocalAuthority;

public class GetLocalAuthorityRequest
{
    public string Code { get; set; }

    public GetLocalAuthorityRequest(string code)
    {
        Code = code;
    }
}