namespace ASP.Domain.Repositories.Establishments.DAO;

public class LinkTypeDAO
{
    public string Code { get; }
    public string Name { get; }

    public LinkTypeDAO(string code, string name)
    {
        Code = code;
        Name = name;
    }
}