using ASP.Application.UseCases.ContentPage.UpdateContentTemplate;
using ASP.Application.UseCases.ContentPage.ViewContentTemplate;
using ASP.Application.UseCases.Establishments.EstablishmentSearch;
using ASP.Application.UseCases.Establishments.GetEstablishmentDetails;
using ASP.Core.DTO.Establishment;
using ASP.Core.Results;
using ASP.Core.Search;
using ASP.Core.Templating;

namespace ASP.Web
{
    public interface IAspApi
    {
        Task<Result<ContentTemplate>> ViewContentTemplate(ViewContentTemplateRequest request);
        Task<Result<UpdateContentTemplateResponse>> UpdateContentTemplate(UpdateContentTemplateRequest request);
        Task<Result<EstablishmentDetailsDTO>> GetEstablishmentDetails(GetEstablishmentDetailsUseCaseRequest request);
        Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> EstablishmentSearch(EstablishmentSearchUseCaseRequest request);
    }
}