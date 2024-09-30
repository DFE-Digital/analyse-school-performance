using ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.Establishments.Search;
using ASP.Core.Establishments.SearchSuggestions;
using ASP.Core.Results;
using ASP.Core.Templating;
using ASP.Application.UseCases.Establishments.DTO;
using ASP.Application.UseCases.Establishments.GetAllEstablishments;
using ASP.Application.UseCases.LocalAuthorities.GetLocalAuthority;
using ASP.Application.UseCases.MultiAcademyTrusts.GetMultiAcademyTrust;
using ASP.Core.Scoping;

namespace ASP.Application
{
    public interface IAspApiClient
    {
        Task<Result<ContentTemplate>> ViewContentTemplate(ViewContentTemplateRequest request);
        Task<Result<Done>> UpdateContentTemplate(UpdateContentTemplateRequest request);
        Task<Result<List<ContentTemplate>>> GetAllContentTemplates();
        Task<Result<EstablishmentDetailsDTO>> GetEstablishmentDetails(GetEstablishmentDetailsRequest request);
        Task<Result<SearchResultsPage<EstablishmentListingDTO>>> EstablishmentSearch(EstablishmentSearchRequest request);
        Task<Result<SearchSuggestionsResult<EstablishmentSuggestionDTO>>> EstablishmentSearchSuggestions(EstablishmentSearchSuggestionsRequest request);
        Task<Result<UseCases.LocalAuthorities.DTO.LocalAuthorityDTO>> GetLocalAuthority(GetLocalAuthorityRequest request);
        Task<Result<UseCases.MultiAcademyTrusts.DTO.MultiAcademyTrustDTO>> GetMultiAcademyTrust(GetMultiAcademyTrustRequest request);
        Task<Result<ScopedResultsPage<EstablishmentListingDTO>>> GetAllEstablishments(GetAllEstablishmentsRequest request);
    }
}
