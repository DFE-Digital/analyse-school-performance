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
    public class UseCaseReferenceApi : IAspApi
    {
        private readonly IViewContentTemplateUseCase _viewContentTemplate;
        private readonly IUpdateContentTemplateUseCase _updateContentTemplate;
        private readonly IGetEstablishmentDetailsUseCase _getEstablishmentDetails;
        private readonly IEstablishmentSearchUseCase _establishmentSearch;

        public UseCaseReferenceApi(IViewContentTemplateUseCase viewContentTemplate, IUpdateContentTemplateUseCase updateContentTemplate, IGetEstablishmentDetailsUseCase getEstablishmentDetails, IEstablishmentSearchUseCase establishmentSearch)
        {
            _viewContentTemplate = viewContentTemplate;
            _updateContentTemplate = updateContentTemplate;
            _getEstablishmentDetails = getEstablishmentDetails;
            _establishmentSearch = establishmentSearch;
        }

        public Task<Result<ContentTemplate>> ViewContentTemplate(ViewContentTemplateRequest request)
        {
            return _viewContentTemplate.HandleRequest(request);
        }

        public Task<Result<UpdateContentTemplateResponse>> UpdateContentTemplate(UpdateContentTemplateRequest request)
        {
            return _updateContentTemplate.HandleRequest(request);
        }

        public Task<Result<EstablishmentDetailsDTO>> GetEstablishmentDetails(GetEstablishmentDetailsUseCaseRequest request)
        {
            return _getEstablishmentDetails.HandleRequest(request);
        }

        public Task<Result<SearchResult<EstablishmentDetailsSearchResultDTO>>> EstablishmentSearch(EstablishmentSearchUseCaseRequest request)
        {
            return _establishmentSearch.HandleRequest(request);
        }
    }
}