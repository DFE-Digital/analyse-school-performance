using ASP.Core.Templating;
using ASP.Core.Templating.Repository;
using ErrorOr;

namespace ASP.Application.UseCases.ViewContentTemplate
{
    public class ViewContentTemplateUseCase : IViewContentTemplateUseCase
    {
        private readonly IContentTemplateRepository _repository;

        public ViewContentTemplateUseCase(IContentTemplateRepository pageContentRepository)
        {
            _repository = pageContentRepository ??
                throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public async Task<ErrorOr<ContentTemplate>> HandleRequest(ViewContentTemplateRequest request)
        {
            return await _repository.Get(request.ContentTemplateId);
        }
    }
}
