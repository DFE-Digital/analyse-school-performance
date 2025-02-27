namespace ASP.Api.Functions.MultiAcademyTrusts;

public static class DomainMappingExtensions
{
    public static Client.LookupValueWithUid ForApiClient(this Domain.MultiAcademyTrusts.MultiAcademyTrust multiAcademyTrust)
    {
        return new Client.LookupValueWithUid(multiAcademyTrust.Uid, multiAcademyTrust.Name);
    }
}
