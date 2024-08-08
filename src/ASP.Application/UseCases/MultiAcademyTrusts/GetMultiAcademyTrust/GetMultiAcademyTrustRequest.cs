namespace ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;

public class GetMultiAcademyTrustRequest
{
    public string Id { get; set; }

    public GetMultiAcademyTrustRequest(string id)
    {
        Id = id;
    }
}