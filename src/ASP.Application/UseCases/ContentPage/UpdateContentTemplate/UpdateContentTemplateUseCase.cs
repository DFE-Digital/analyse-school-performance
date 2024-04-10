using ASP.Core.Results;
using ASP.Core.Templating;

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

        public async Task<Result<UpdateContentTemplateResponse>> HandleRequest(UpdateContentTemplateRequest request)
        {
            return await _repository.Update(request.ContentTemplateId, request.ContentTemplate)
                .Map(_ => new UpdateContentTemplateResponse());
        }
    }
}
