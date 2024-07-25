using ASP.Application.UseCases.ContentPage.GetAllContentTemplates;
using ASP.Core.Results;
using ASP.Core.Templating;

namespace ASP.Application.UseCases.ContentTemplates.GetAllContentTemplates
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
