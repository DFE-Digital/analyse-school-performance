namespace ASP.Core.MultiAcademyTrusts;

public class MultiAcademyTrust
{
    public MultiAcademyTrust(string id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public string Id { get; }
    public string Name { get; }
}