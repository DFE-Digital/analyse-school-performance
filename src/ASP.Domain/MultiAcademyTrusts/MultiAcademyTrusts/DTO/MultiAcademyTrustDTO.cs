namespace ASP.Domain.MultiAcademyTrusts.UseCases.DTO;

public class MultiAcademyTrustDTO
{
    public MultiAcademyTrustDTO(string id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public string Id { get; }
    public string Name { get; }
}