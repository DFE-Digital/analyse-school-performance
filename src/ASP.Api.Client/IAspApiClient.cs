using ASP.Api.Client.DataDownloads;
using ASP.Api.Client.Establishments;
using ASP.Api.Client.LocalAuthorities;
using ASP.Api.Client.MultiAcademyTrusts;
using ASP.Api.Client.Templating;
using ASP.Core.Network;
using ASP.Core.Pagination;
using ASP.Core.Results;

namespace ASP.Api.Client;

public interface IAspApiClient
{
    Task<Result<List<ContentTemplate>>> GetAllContentTemplates(GetAllContentTemplatesRequest request);
    Task<Result<ContentTemplate>> GetContentTemplate(ViewContentTemplateRequest request);
    Task<Result<Done>> UpdateContentTemplate(UpdateContentTemplateRequest request);

    Task<Result<GetAvailableDownloadsResponse>> GetAvailableDownloads(GetAvailableDownloadsRequest request);
    Task<Result<FileStreamResponse>> GetDownloadPackage(GetDownloadPackageRequest request);

    Task<Result<EstablishmentDetails>> GetEstablishmentDetails(GetEstablishmentDetailsRequest request);
    Task<Result<ScopedResultsPage<EstablishmentListing>>> GetAllEstablishments(GetAllEstablishmentsRequest request);
    Task<Result<ScopedSearchResultsPage<EstablishmentListing>>> EstablishmentSearch(EstablishmentSearchRequest request);
    Task<Result<ScopedSearchSuggestionsList<EstablishmentSuggestion>>> EstablishmentSearchSuggestions(EstablishmentSearchSuggestionsRequest request);
    Task<Result<IsEstablishmentAccessibleInScopeResponse>> IsEstablishmentAccessibleInScope(IsEstablishmentAccessibleInScopeRequest request);
    Task<Result<GetLinkedEstablishmentsResponse>> GetLinkedEstablishments(GetLinkedEstablishmentsRequest request);

    Task<Result<LookupValueWithCode>> GetLocalAuthority(GetLocalAuthorityRequest request);
    Task<Result<ResultsPage<LookupValueWithCode>>> GetAllLocalAuthorities(GetAllLocalAuthoritiesRequest request);
    Task<Result<SearchResultsPage<LookupValueWithCode>>> LocalAuthoritySearch(LocalAuthoritySearchRequest request);
    Task<Result<SearchSuggestionsList<LookupValueWithCode>>> LocalAuthoritySearchSuggestions(LocalAuthoritySearchSuggestionsRequest request); 
    
    Task<Result<LookupValueWithId>> GetMultiAcademyTrust(GetMultiAcademyTrustRequest request);
}
