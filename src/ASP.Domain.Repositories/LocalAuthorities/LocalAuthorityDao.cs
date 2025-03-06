namespace ASP.Domain.Repositories.LocalAuthorities;

public class LocalAuthorityDao
{
    public LocalAuthorityDao(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public string Code { get; set; }
    public string Name { get; set; }
}