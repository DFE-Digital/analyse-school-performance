using ASP.Core.PageContent;
using ASP.Core.PageContent.Repository;
using ErrorOr;

namespace ASP.Application.UseCases.ViewContentPage
{
    public class ViewContentPageUseCase : IViewContentPageUseCase
    {
        private readonly IPageContentRepository _pageContentRepository;

        public ViewContentPageUseCase(IPageContentRepository pageContentRepository)
        {
            _pageContentRepository = pageContentRepository ??
                throw new ArgumentNullException(nameof(pageContentRepository));
        }

        public async Task<ErrorOr<PageContentTemplate>> HandleRequest(ViewContentPageRequest request)
        {
            return await _pageContentRepository.Get(request.PageContentId);
        }
    }
}
