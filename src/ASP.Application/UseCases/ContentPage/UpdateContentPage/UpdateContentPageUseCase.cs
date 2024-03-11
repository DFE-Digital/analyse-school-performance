using ASP.Core.PageContent.Repository;
using ErrorOr;

namespace ASP.Application.UseCases.UpdateContentPage
{
    public class UpdateContentPageUseCase : IUpdateContentPageUseCase
    {
        private readonly IPageContentRepository _pageContentRepository;

        public UpdateContentPageUseCase(IPageContentRepository pageContentRepository)
        {
            _pageContentRepository = pageContentRepository ??
                throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public async Task<ErrorOr<UpdateContentPageResponse>> HandleRequest(UpdateContentPageRequest request)
        {
            var updatedPageContent = request.PageContentTemplate;
            updatedPageContent.id = request.PageContentId;
            updatedPageContent.contentId = request.PageContentId;

            return await _pageContentRepository.Update(updatedPageContent)
                .Then(_ => new UpdateContentPageResponse());
        }
    }
}
