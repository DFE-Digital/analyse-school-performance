namespace ASP.Domain.Establishments.LinkedEstablishments;

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