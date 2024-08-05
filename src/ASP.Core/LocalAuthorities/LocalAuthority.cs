namespace ASP.Core.LocalAuthorities;

public class LocalAuthority
{
    public LocalAuthority(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public string Code { get; set; }
    public string Name { get; set; }
}