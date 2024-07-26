namespace ASP.Infrastructure.DAO.LocalAuthority;

public class LocalAuthorityDAO
{
    public LocalAuthorityDAO(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public string Code { get; set; }
    public string Name { get; set; }
}