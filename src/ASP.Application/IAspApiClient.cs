using ASP.Application.UseCases.ContentTemplates.UpdateContentTemplate;
using ASP.Application.UseCases.ContentTemplates.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.EstablishmentSearchSuggestions;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.DTO.Establishment;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Search.Suggestions;
using ASP.Core.Templating;

namespace ASP.Application
{
    public interface IAspApiClient
    {
        Task<Result<ContentTemplate>> ViewContentTemplate(ViewContentTemplateRequest request);
        Task<Result<Done>> UpdateContentTemplate(UpdateContentTemplateRequest request);
        Task<Result<List<ContentTemplate>>> GetAllContentTemplates();
        Task<Result<EstablishmentDetailsDTO>> GetEstablishmentDetails(GetEstablishmentDetailsRequest request);
        Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> EstablishmentSearch(EstablishmentSearchRequest request);
        Task<Result<SearchSuggestionsResult<EstablishmentSearchSuggestionsResultDTO>>> EstablishmentSearchSuggestions(EstablishmentSearchSuggestionsRequest request);
    }
}
