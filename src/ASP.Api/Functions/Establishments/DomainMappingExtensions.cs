using ASP.Core.Pagination;
using ASP.Domain.Establishments;
using ASP.Domain.Establishments.LinkedEstablishments;

namespace ASP.Api.Functions.Establishments;

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

    public static Client.Establishments.AgeRange? ForApiClient(this Domain.Establishments.AgeRange? ageRange)
    {
        if (ageRange == null) return null;  // Return null directly instead of an empty object

        return new Client.Establishments.AgeRange(
            ageRange.Low,
            ageRange.High);
    }

    public static Client.Establishments.EstablishmentDetails ForApiClient(this Domain.Establishments.EstablishmentDetails details)
    {
        return new Client.Establishments.EstablishmentDetails()
        {
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

    public static Client.Establishments.EstablishmentListing ForApiClient(
        this Domain.Establishments.EstablishmentListing details)
    {
        return new Client.Establishments.EstablishmentListing()
        {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.ForApiClient(),
            EducationPhase = details.EducationPhase.ToString(),
            Laestab = details.Laestab
        };
    }

    public static Client.Establishments.EstablishmentSuggestion ForApiClient(
        this Domain.Establishments.SearchSuggestions.EstablishmentSuggestion details)
    {
        return new Client.Establishments.EstablishmentSuggestion()
        {
            Urn = details.Urn,
            Name = details.Name,
            Address = details.Address.ForApiClient(),
            Laestab = details.Laestab
        };
    }

    public static List<Client.Establishments.EstablishmentSuggestion> ForApiClient(
        this IEnumerable<Domain.Establishments.SearchSuggestions.EstablishmentSuggestion> detailsList)
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

    public static Client.Establishments.HeadTeacher? ForApiClient(this Domain.Establishments.HeadTeacher? headTeacher)
    {
        if (headTeacher == null) return null;  // Return null directly instead of an empty object

        return new Client.Establishments.HeadTeacher()
        {
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

    public static Client.Establishments.IsEstablishmentAccessibleInScopeResponse? ForApiClient(this Domain.Establishments.UseCases.IsEstablishmentAccessibleInScope.IsEstablishmentAccessibleInScopeResponse? response)
    {
        if (response == null) return null;  // Return null directly instead of an empty object

        return new Client.Establishments.IsEstablishmentAccessibleInScopeResponse(
            response.Urn,
            response.Scope,
            response.ScopeIdentifier,
            response.IsAccessible);
    }

    public static ScopedSearchResultsPage<Client.Establishments.EstablishmentListing>? ForApiClient(this ScopedSearchResultsPage<Domain.Establishments.EstablishmentListing>? response)
    {
        if (response == null) return null;  // Return null directly instead of an empty object

        return response.Map(r => r.ForApiClient());
    }

    public static ScopedSearchSuggestionsList<Client.Establishments.EstablishmentSuggestion>? ForApiClient(this ScopedSearchSuggestionsList<Domain.Establishments.SearchSuggestions.EstablishmentSuggestion>? response)
    {
        if (response == null) return null;  // Return null directly instead of an empty object

        return new ScopedSearchSuggestionsList<Client.Establishments.EstablishmentSuggestion>
        {
            Suggestions = response.Suggestions.ForApiClient(),
            MaxSuggestions = response.MaxSuggestions,
            SearchTerm = response.SearchTerm,
            Scope = response.Scope,
            ScopeIdentifier = response.ScopeIdentifier
        };
    }

    public static ScopedResultsPage<Client.Establishments.EstablishmentListing>? ForApiClient(this ScopedResultsPage<Domain.Establishments.EstablishmentListing>? response)
    {
        if (response == null) return null;  // Return null directly instead of an empty object

        return response.Map(r => r.ForApiClient());
    }

    public static List<Client.Establishments.LinkedEstablishment> ForApiClient(
        this List<LinkedEstablishment>? linkedEstablishment)
    {
        if (linkedEstablishment == null) return new List<Client.Establishments.LinkedEstablishment>();

        return linkedEstablishment.Select(x => new Client.Establishments.LinkedEstablishment()
        {
            Name = x.Name,
            Urn = x.Urn
        }).ToList();
    }

    public static Client.Establishments.GetLinkedEstablishmentsResponse ForApiClient(
        this Domain.Establishments.UseCases.GetLinkedEstablishments.GetLinkedEstablishmentsResponse linkedEstablishmentsResponse)
    {
        return new Client.Establishments.GetLinkedEstablishmentsResponse()
        {
            Urn = linkedEstablishmentsResponse.Urn,
            Name = linkedEstablishmentsResponse.Name,
            LinkedUrns = linkedEstablishmentsResponse.LinkedUrns,
            Links = linkedEstablishmentsResponse.Links.ForApiClient()
        };
    }

    public static List<Client.Establishments.EstablishmentLink> ForApiClient(
        this List<EstablishmentLink> establishmentLink)
    {
        return establishmentLink.Select(x => new Client.Establishments.EstablishmentLink()
        {
            Date = x.Date?.ToString("yyyy-MM-dd"),
            LinkType = x.LinkType.ForApiClient(),
            Establishments = x.Establishments.ForApiClient(),
            Description = x.Description
        }).ToList();
    }

    public static Client.LookupValueWithCode? ForApiClient(
        this LinkType? linkType)
    {
        if (linkType == null) return null;

        return new Client.LookupValueWithCode(
            linkType.Code,
            linkType.Name
        );
    }

    public static Client.LookupValueWithId? ForApiClient(this MultiAcademyTrust? multiAcademyTrust)
    {
        if (multiAcademyTrust is null) return null;

        return new Client.LookupValueWithId(multiAcademyTrust.Id, multiAcademyTrust.Name);
    }

    public static Client.LookupValueWithCode? ForApiClient(this Diocese? diocese)
    {
        if (diocese is null) return null;

        return new Client.LookupValueWithCode(diocese.Id, diocese.Name);
    }
}