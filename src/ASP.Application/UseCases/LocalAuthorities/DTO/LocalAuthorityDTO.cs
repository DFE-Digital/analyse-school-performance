namespace ASP.Application.UseCases.LocalAuthorities.DTO;

public class LocalAuthorityDTO
{
    public LocalAuthorityDTO(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public string Code { get; set; }
    public string Name { get; set; }
}