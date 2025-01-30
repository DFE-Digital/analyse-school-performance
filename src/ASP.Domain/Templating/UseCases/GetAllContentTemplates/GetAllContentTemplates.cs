using ASP.Core.Results;

namespace ASP.Domain.Templating.UseCases.GetAllContentTemplates
{
    public class GetAllContentTemplates : IGetAllContentTemplates
    {
        private readonly IContentTemplateRepository _repository;

        public GetAllContentTemplates(IContentTemplateRepository pageContentRepository)
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
