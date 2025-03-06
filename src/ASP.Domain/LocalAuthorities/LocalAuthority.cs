namespace ASP.Domain.LocalAuthorities;

public class LocalAuthority
{
    public LACode Code { get; }
    public string Name { get; }

    public LocalAuthority(LACode code, string name)
    {
        Code = code;
        Name = name;
    }
}