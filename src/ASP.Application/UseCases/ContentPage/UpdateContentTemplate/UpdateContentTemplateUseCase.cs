using ASP.Core.Templating.Repository;
using ErrorOr;

namespace ASP.Application.UseCases.UpdateContentTemplate
{
    public class UpdateContentTemplateUseCase : IUpdateContentTemplateUseCase
    {
        private readonly IContentTemplateRepository _repository;

        public UpdateContentTemplateUseCase(IContentTemplateRepository pageContentRepository)
        {
            _repository = pageContentRepository ??
                throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public async Task<ErrorOr<UpdateContentTemplateResponse>> HandleRequest(UpdateContentTemplateRequest request)
        {
            return await _repository.Update(request.ContentTemplateId, request.ContentTemplate)
                .Then(_ => new UpdateContentTemplateResponse());
        }
    }
}
