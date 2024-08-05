namespace ASP.Infrastructure.Establishments.DAO;

public class OfstedRatingDAO
{
    public string Code { get; }
    public string Name { get; }

    public OfstedRatingDAO(string code, string name)
    {
        Code = code;
        Name = name;
    }
}