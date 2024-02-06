using ASP.Core.PageContent.Repository;

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

        public async Task<ViewContentPageResponse> HandleRequest(ViewContentPageRequest request)
        {
            try
            {
                var response = await _pageContentRepository.Get(request.PageContentId);
                if (response.IsError)
                {
                    throw new NotImplementedException(response.FirstError.Description);
                }

                return new ViewContentPageResponse { PageContentTemplate = response.Value };
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }

        }
    }
}
