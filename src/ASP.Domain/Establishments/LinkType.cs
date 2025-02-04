namespace ASP.Domain.Establishments;

public class LinkType
{
    public string Code { get; }
    public string Name { get; }

    public LinkType(string code, string name)
    {
        Code = code;
        Name = name;
    }
}