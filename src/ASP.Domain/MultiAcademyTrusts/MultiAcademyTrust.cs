namespace ASP.Domain.MultiAcademyTrusts;

public class MultiAcademyTrust
{
    public MultiAcademyTrust(string id, string name)
    {
        Uid = id;
        Name = name;
    }
    
    public string Uid { get; }
    public string Name { get; }
}