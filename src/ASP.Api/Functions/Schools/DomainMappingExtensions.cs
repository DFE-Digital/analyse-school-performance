using ASP.Core.Pagination;

namespace ASP.Api.Functions.Schools;

public static class DomainMappingExtensions
{
    public static string? ForApiClient(this Domain.Establishments.Address? address)
    {
        if (address == null) return null;  // Return null directly instead of an empty object

        return address.ToString();
    }

    public static Client.LookupValueWithCode? ForApiClient(this Domain.Establishments.AdmissionsPolicy? admissionsPolicy)
    {
        if (admissionsPolicy == null) return null;  // Return null directly instead of an empty object

        return new Client.LookupValueWithCode(
            admissionsPolicy.Code,
            admissionsPolicy.Name);
    }

    public static Client.Schools.AgeRange? ForApiClient(this Domain.Establishments.AgeRange? ageRange)
    {
        if (ageRange == null) return null;  // Return null directly instead of an empty object

        return new Client.Schools.AgeRange(
            ageRange.Low,
            ageRange.High);
    }

    public static Client.Schools.SchoolDetails ForApiClient(this Domain.Establishments.EstablishmentDetails details)
    {
        return new Client.Schools.SchoolDetails() {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.ForApiClient(),
            EducationPhase = details.EducationPhase.ToString(),
            ReligiousDenomination = details.ReligiousDenomination.ForApiClient(),
            AdmissionsPolicy = details.AdmissionsPolicy.ForApiClient(),
            LocalAuthority = details.LocalAuthority.ForApiClient(),
            HeadTeacher = details.HeadTeacher.ForApiClient(),
            AgeRange = details.AgeRange.ForApiClient(),
            EstablishmentType = details.EstablishmentType.ForApiClient(),
            Gender = details.Gender.ForApiClient(),
            ResourcedProvisionType = details.ResourcedProvisionType.ForApiClient(),
            NoOfPupils = details.NoOfPupils,
            Laestab = details.Laestab,
            MultiAcademyTrust = details.MultiAcademyTrust.ForApiClient(),
            Diocese = details.Diocese.ForApiClient(),
        };
    }

    public static Client.Schools.SchoolListing ForApiClient(this Domain.Establishments.EstablishmentListing details)
    {
        return new Client.Schools.SchoolListing() {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.ForApiClient(),
            EducationPhase = details.EducationPhase.ToString(),
            Laestab = details.Laestab
        };
    }

    public static Client.Schools.SchoolSuggestion ForApiClient(this Domain.Establishments.SearchSuggestions.EstablishmentSuggestion details)
    {
        return new Client.Schools.SchoolSuggestion() {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.ForApiClient(),
            Laestab = details.Laestab
        };
    }

    public static List<Client.Schools.SchoolSuggestion> ForApiClient(this IEnumerable<Domain.Establishments.SearchSuggestions.EstablishmentSuggestion> detailsList)
    {
        return detailsList.Select(ForApiClient).ToList();
    }

    public static Client.LookupValueWithCode? ForApiClient(this Domain.Establishments.EstablishmentType? establishmentType)
    {
        if (establishmentType == null) return null;  // Return null directly instead of an empty object

        return new Client.LookupValueWithCode(
            establishmentType.Code,
            establishmentType.Name);
    }

    public static Client.LookupValueWithCode? ForApiClient(this Domain.Establishments.Gender? gender)
    {
        if (gender == null) return null;  // Return null directly instead of an empty object

        return new Client.LookupValueWithCode(
            gender.Code,
            gender.Name);
    }

    public static Client.Schools.HeadTeacher? ForApiClient(this Domain.Establishments.HeadTeacher? headTeacher)
    {
        if (headTeacher == null) return null;  // Return null directly instead of an empty object

        return new Client.Schools.HeadTeacher() {
            FirstName = headTeacher.FirstName,
            LastName = headTeacher.LastName,
            PreferredJobTitle = headTeacher.PreferredJobTitle,
            Title = headTeacher.Title
        };
    }

    public static Client.LookupValueWithCode? ForApiClient(this Domain.Establishments.LocalAuthority? localAuthority)
    {
        if (localAuthority == null) return null;  // Return null directly instead of an empty object

        return new Client.LookupValueWithCode(
            localAuthority.Code,
            localAuthority.Name);
    }

    public static Client.LookupValueWithCode? ForApiClient(this Domain.Establishments.ReligiousDenomination? religiousDenomination)
    {
        if (religiousDenomination == null) return null;  // Return null directly instead of an empty object

        return new Client.LookupValueWithCode(
            religiousDenomination.Code,
            religiousDenomination.Name);
    }

    public static Client.LookupValueWithCode? ForApiClient(this Domain.Establishments.ResourcedProvisionType? resourcedProvisionType)
    {
        if (resourcedProvisionType == null) return null;  // Return null directly instead of an empty object

        return new Client.LookupValueWithCode(
            resourcedProvisionType.Code,
            resourcedProvisionType.Name);
    }

    public static Client.Schools.SchoolsGetAccessResponse? ForApiClient(this Domain.Establishments.UseCases.IsEstablishmentAccessibleInScope.IsEstablishmentAccessibleInScopeResponse? response)
    {
        if (response == null) return null;  // Return null directly instead of an empty object

        return new Client.Schools.SchoolsGetAccessResponse(
            response.IsAccessibleInScope,
            response.IsAccessibleViaLinkedSchools);
    }

    public static ResultsPage<Client.Schools.SchoolListing>? ForApiClient(this ResultsPage<Domain.Establishments.EstablishmentListing>? response)
    {
        if (response == null) return null;  // Return null directly instead of an empty object

        return response.Map(r => r.ForApiClient());
    }

    public static List<Client.Schools.LinkedSchool> ForApiClient(this List<Domain.Establishments.LinkedEstablishments.LinkedEstablishment>? linkedEstablishment)
    {
        if (linkedEstablishment == null) return new List<Client.Schools.LinkedSchool>();

        return linkedEstablishment.Select(x => new Client.Schools.LinkedSchool() {
            Name = x.Name,
            Urn = x.Urn
        }).ToList();
    }

    public static Client.Schools.SchoolsGetLinkedSchoolsResponse ForApiClient(this Domain.Establishments.UseCases.GetLinkedEstablishments.GetLinkedEstablishmentsResponse linkedEstablishmentsResponse)
    {
        return new Client.Schools.SchoolsGetLinkedSchoolsResponse() {
            LinkedUrns = linkedEstablishmentsResponse.LinkedUrns,
            Links = linkedEstablishmentsResponse.Links.ForApiClient()
        };
    }

    public static List<Client.Schools.SchoolLink> ForApiClient(this List<Domain.Establishments.LinkedEstablishments.EstablishmentLink> establishmentLink)
    {
        return establishmentLink.Select(x => new Client.Schools.SchoolLink() {
            Date = x.Date?.ToString("yyyy-MM-dd"),
            LinkType = x.LinkType.ForApiClient(),
            Establishments = x.Establishments.ForApiClient(),
            Description = x.Description
        }).ToList();
    }

    public static Client.LookupValueWithCode? ForApiClient(this Domain.Establishments.LinkedEstablishments.LinkType? linkType)
    {
        if (linkType == null) return null;

        return new Client.LookupValueWithCode(
            linkType.Code,
            linkType.Name
        );
    }

    public static Client.LookupValueWithId? ForApiClient(this Domain.Establishments.MultiAcademyTrust? multiAcademyTrust)
    {
        if (multiAcademyTrust is null) return null;

        return new Client.LookupValueWithId(multiAcademyTrust.Id, multiAcademyTrust.Name);
    }

    public static Client.LookupValueWithCode? ForApiClient(this Domain.Establishments.Diocese? diocese)
    {
        if (diocese is null) return null;

        return new Client.LookupValueWithCode(diocese.Id, diocese.Name);
    }
}