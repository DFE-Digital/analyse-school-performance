using ASP.Core.Pagination;
using ASP.Core.Results;
using ASP.Domain;
using ASP.Domain.DataDownloads.UseCases.GetAvailableDownloads;
using ASP.Domain.DataDownloads.UseCases.GetDownloadPackage;
using ASP.Domain.Establishments.Search;
using ASP.Domain.Establishments.SearchSuggestions;
using ASP.Domain.Establishments.UseCases.DTO;
using ASP.Domain.Establishments.UseCases.EstablishmentSearch;
using ASP.Domain.Establishments.UseCases.EstablishmentSearchSuggestions;
using ASP.Domain.Establishments.UseCases.GetAllEstablishments;
using ASP.Domain.Establishments.UseCases.GetEstablishmentDetails;
using ASP.Domain.LocalAuthorities.LocalAuthoritySearchSuggestions;
using ASP.Domain.LocalAuthorities.UseCases.GetAllLocalAuthorities;
using ASP.Domain.LocalAuthorities.UseCases.GetLocalAuthority;
using ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearch;
using ASP.Domain.LocalAuthorities.UseCases.LocalAuthoritySearchSuggestions;
using ASP.Domain.MultiAcademyTrusts.UseCases.GetMultiAcademyTrust;
using ASP.Domain.Templating;
using ASP.Domain.Templating.UseCases.UpdateContentTemplate;
using ASP.Domain.Templating.UseCases.ViewContentTemplate;
using LA = ASP.Domain.LocalAuthorities.UseCases.DTO;
using MAT = ASP.Domain.MultiAcademyTrusts.UseCases.DTO;

namespace ASP.Application
{
    public interface IAspApiClient
    {
        Task<Result<ContentTemplate>> ViewContentTemplate(ViewContentTemplateRequest request);
        Task<Result<Done>> UpdateContentTemplate(UpdateContentTemplateRequest request);
        Task<Result<List<ContentTemplate>>> GetAllContentTemplates();
        Task<Result<EstablishmentDetailsDTO>> GetEstablishmentDetails(GetEstablishmentDetailsRequest request);
        Task<Result<GetAvailableDownloadsResponse>> GetAvailableDownloads(GetAvailableDownloadsRequest request);
        Task<Result<FileStreamResponse>> GetDownloadPackage(GetDownloadPackageRequest request);
        Task<Result<ScopedSearchResultsPage<EstablishmentListingDTO>>> EstablishmentSearch(EstablishmentSearchRequest request);
        Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>> EstablishmentSearchSuggestions(EstablishmentSearchSuggestionsRequest request);
        Task<Result<LA.LocalAuthorityDTO>> GetLocalAuthority(GetLocalAuthorityRequest request);
        Task<Result<MAT.MultiAcademyTrustDTO>> GetMultiAcademyTrust(GetMultiAcademyTrustRequest request);
        Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> GetAllEstablishments(GetAllEstablishmentsRequest request);
        Task<Result<ResultsPage<LA.LocalAuthorityDTO>>> GetAllLocalAuthorities(GetAllLocalAuthoritiesRequest request);
        Task<Result<SearchResultsPage<LA.LocalAuthorityDTO>>> LocalAuthoritySearch(LocalAuthoritySearchRequest request);
        Task<Result<LocalAuthoritySearchSuggestionsResult<LA.LocalAuthorityDTO>>> LocalAuthoritySearchSuggestions(LocalAuthoritySearchSuggestionsRequest request);
    }
}
