namespace ASP.Api.Functions.MultiAcademyTrusts;

public static class DomainMappingExtensions
{
    public static Client.LookupValueWithId ForApiClient(this Domain.MultiAcademyTrusts.MultiAcademyTrust multiAcademyTrust)
    {
        return new Client.LookupValueWithId(multiAcademyTrust.Id, multiAcademyTrust.Name);
    }
}
