namespace ASP.Infrastructure.MultiAcademyTrusts.DAO;

public class MultiAcademyTrustDAO
{
    public MultiAcademyTrustDAO(string id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public string Id { get; }
    public string Name { get; }
}