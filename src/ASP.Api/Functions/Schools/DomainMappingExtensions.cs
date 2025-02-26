using ASP.Core.Pagination;

namespace ASP.Api.Functions.Schools;

public static class DomainMappingExtensions
{
    public static Client.Schools.SchoolDetails ForApiClientAsDetails(this Domain.Schools.Details.SchoolWithEstablishmentDetails school)
    {
        return new Client.Schools.SchoolDetails() {
            Urn = school.Urn.Value,
            Name = school.Name,
            Address = school.Address.MapNullable(ForApiClient),
            EducationPhase = school.EducationPhase.ToString(),
            ReligiousDenomination = school.EstablishmentDetails.ReligiousDenomination.MapNullable(ForApiClient),
            AdmissionsPolicy = school.EstablishmentDetails.AdmissionsPolicy.MapNullable(ForApiClient),
            LocalAuthority = school.LocalAuthority.MapNullable(ForApiClient),
            HeadTeacher = school.EstablishmentDetails.HeadTeacher.MapNullable(ForApiClient),
            AgeRange = school.EstablishmentDetails.AgeRange.MapNullable(ForApiClient),
            EstablishmentType = school.EstablishmentDetails.EstablishmentType.MapNullable(ForApiClient),
            Gender = school.EstablishmentDetails.Gender.MapNullable(ForApiClient),
            ResourcedProvisionType = school.EstablishmentDetails.ResourcedProvisionType.MapNullable(ForApiClient),
            NoOfPupils = school.EstablishmentDetails.NoOfPupils,
            Laestab = school.LAEstab.Value,
            MultiAcademyTrust = school.MultiAcademyTrust.MapNullable(ForApiClient),
            Diocese = school.Diocese.MapNullable(ForApiClient),
        };
    }

    public static Client.Schools.SchoolListing ForApiClientAsListing(this Domain.Schools.School school)
    {
        return new Client.Schools.SchoolListing() {
            Urn = school.Urn.Value,
            Name = school.Name,
            Address = school.Address.MapNullable(ForApiClient),
            EducationPhase = school.EducationPhase.ToString(),
            Laestab = school.LAEstab.Value
        };
    }

    public static Client.Schools.SchoolSuggestion ForApiClientAsSuggestion(this Domain.Schools.School school)
    {
        return new Client.Schools.SchoolSuggestion() {
            Urn = school.Urn.Value,
            Name = school.Name,
            Address = school.Address.MapNullable(ForApiClient),
            Laestab = school.LAEstab.Value
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

    public static Client.LookupValueWithId ForApiClient(this Domain.Schools.MultiAcademyTrust multiAcademyTrust)
    {
        return new Client.LookupValueWithId(multiAcademyTrust.Uid, multiAcademyTrust.Name);
    }

    public static Client.LookupValueWithCode ForApiClient(this Domain.Schools.Diocese diocese)
    {
        return new Client.LookupValueWithCode(diocese.Id, diocese.Name);
    }

    public static string ForApiClient(this Domain.Schools.Address address)
    {
        return address.ToString();
    }

    public static Client.Schools.HeadTeacher ForApiClient(this Domain.Schools.Details.HeadTeacher headTeacher)
    {
        return new Client.Schools.HeadTeacher() {
            FirstName = headTeacher.FirstName,
            LastName = headTeacher.LastName,
            PreferredJobTitle = headTeacher.PreferredJobTitle,
            Title = headTeacher.Title
        };
    }

    public static Client.Schools.AgeRange ForApiClient(this Domain.Schools.Details.AgeRange ageRange)
    {
        return new Client.Schools.AgeRange(
            ageRange.Low,
            ageRange.High);
    }

    public static Client.LookupValueWithCode ForApiClient(this Domain.LookupValue lookupValue)
    {
        return new Client.LookupValueWithCode(
            lookupValue.Code,
            lookupValue.Name);
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