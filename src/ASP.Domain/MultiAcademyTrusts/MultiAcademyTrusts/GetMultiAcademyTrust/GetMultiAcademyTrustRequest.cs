namespace ASP.Domain.MultiAcademyTrusts.UseCases.GetMultiAcademyTrust;

public class GetMultiAcademyTrustRequest
{
    public string Id { get; set; }

    public GetMultiAcademyTrustRequest(string id)
    {
        Id = id;
    }
}