using ASP.Core.Results;
using ASP.Core.Templating;

namespace ASP.Application.UseCases.ContentPage.GetAllAllContentTemplates
{
    public class GetAllContentTemplatesUseCase : IGetAllContentTemplatesUseCase
    {
        private readonly IContentTemplateRepository _repository;

        public GetAllContentTemplatesUseCase(IContentTemplateRepository pageContentRepository)
        {
            _repository = pageContentRepository ??
                throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public async Task<Result<List<ContentTemplate>>> HandleRequest()
        {
            return await _repository.GetAllPublishedTemplates();
        }
    }
}
