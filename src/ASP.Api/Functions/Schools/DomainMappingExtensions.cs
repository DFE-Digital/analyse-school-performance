using ASP.Core.Pagination;

namespace ASP.Api.Functions.Schools;

public static class DomainMappingExtensions
{
    public static Client.Schools.SchoolDetails ForApiClientAsDetails(this Domain.Schools.Details.SchoolWithEstablishmentDetails school)
    {
        return new Client.Schools.SchoolDetails {
            Urn = school.Urn.Value,
            Laestab = school.LAEstab?.Value ?? "Data not available",
            Name = school.Name,
            Address = school.Address.MapNullable(ForApiClient) ?? "Data not available",
            EducationPhase = school.EducationPhase?.ToString() ?? "Data not available",
            LocalAuthority = school.LocalAuthority.MapNullable(l => l.ForApiClient()),
            MultiAcademyTrust = school.MultiAcademyTrust.MapNullable(mat => mat.Name),
            Diocese = school.Diocese.MapNullable(d => d.Name) ?? "Not applicable",
            ReligiousDenomination = school.EstablishmentDetails.ReligiousDenomination,
            AdmissionsPolicy = school.EstablishmentDetails.AdmissionsPolicy,
            HeadTeacher = school.EstablishmentDetails.HeadTeacher,
            AgeRange = school.EstablishmentDetails.AgeRange,
            EstablishmentType = school.EstablishmentDetails.EstablishmentType,
            Gender = school.EstablishmentDetails.Gender,
            ResourcedProvisionType = school.EstablishmentDetails.ResourcedProvisionType,
            NoOfPupils = school.EstablishmentDetails.NoOfPupils,
        };
    }

    public static Client.Schools.SchoolListing ForApiClientAsListing(this Domain.Schools.School school)
    {
        return new Client.Schools.SchoolListing() {
            Urn = school.Urn.Value,
            Name = school.Name,
            Address = school.Address.MapNullable(ForApiClient) ?? "Data not available",
            EducationPhase = school.EducationPhase?.ToString() ?? "Data not available",
            Laestab = school.LAEstab?.Value ?? "Data not available",
        };
    }

    public static Client.Schools.SchoolSuggestion ForApiClientAsSuggestion(this Domain.Schools.School school)
    {
        return new Client.Schools.SchoolSuggestion() {
            Urn = school.Urn.Value,
            Name = school.Name,
            Address = school.Address.MapNullable(ForApiClient) ?? "Data not available",
            Laestab = school.LAEstab?.Value ?? "Data not available",
        };
    }

    public static Client.Schools.SchoolsGetAccessResponse ForApiClient(this Domain.Schools.UseCases.IsSchoolAccessibleInScope.IsSchoolAccessibleInScopeResponse response)
    {
        return new Client.Schools.SchoolsGetAccessResponse(
            response.IsAccessibleInScope,
            response.IsAccessibleViaLinkedSchools);
    }

    public static Client.Schools.SchoolsGetLinkedSchoolsResponse ForApiClient(this Domain.Schools.UseCases.GetLinkedSchools.GetLinkedSchoolsResponse response)
    {
        return new Client.Schools.SchoolsGetLinkedSchoolsResponse() {
            LinkedUrns = response.LinkedUrns.Select(urn => urn.Value).ToList(),
            Links = response.Links.MapList(ForApiClient)
        };
    }

    public static Client.Schools.LinkedSchool ForApiClientAsLinkedSchool(this Domain.Schools.School school)
    {
        return new Client.Schools.LinkedSchool() {
            Name = school.Name,
            Urn = school.Urn.Value
        };
    }

    public static Client.Schools.SchoolLink ForApiClient(this Domain.Schools.LinkedSchools.LinkedSchoolsLink link)
    {
        return new Client.Schools.SchoolLink() {
            Date = link.Date?.ToString("yyyy-MM-dd"),
            LinkType = link.LinkType.MapNullable(ForApiClient),
            Establishments = link.LinkedSchools.MapList(ForApiClientAsLinkedSchool),
            Description = link.Description
        };
    }

    public static Client.LookupValueWithCode ForApiClient(this Domain.Schools.LinkedSchools.LinkType linkType)
    {
        return new Client.LookupValueWithCode(
            linkType.Code,
            linkType.Name
        );
    }

    public static Client.LookupValueWithCode ForApiClient(this Domain.Schools.LocalAuthority localAuthority)
    {
        return new Client.LookupValueWithCode(
            localAuthority.Code,
            localAuthority.Name);
    }

    public static string ForApiClient(this Domain.Schools.Address address)
    {
        var stringValue = address.ToString();

        return string.IsNullOrWhiteSpace(stringValue) ? "Data not available" : stringValue;
    }

    public static List<U> MapList<T, U>(this IEnumerable<T>? source, Func<T, U> mapFunction)
    {
        if (source == null) return [];

        return source.Select(mapFunction).ToList();
    }

    public static ResultsPage<U> MapResultsPage<T, U>(this ResultsPage<T> source, Func<T, U> mapFunction)
    {
        return source.Map(mapFunction);
    }

    public static U? MapNullable<T, U>(this T? t, Func<T, U> mapFunction)
        where T : notnull
        where U : notnull
    {
        if (t is null) return default;

        return mapFunction(t);
    }

    public static U? MapNullable<T, U>(this T? t, Func<T, U> mapFunction)
        where T : struct
        where U : struct
    {
        if (t is null) return default;

        return mapFunction(t.Value);
    }
}