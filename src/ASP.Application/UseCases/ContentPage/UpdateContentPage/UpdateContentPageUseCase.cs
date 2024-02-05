using ASP.Core.Helpers;
using ASP.Core.PageContent;
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


        public async Task<UpdateContentPageResponse> HandleRequest(UpdateContentPageRequest request)
        {
            try
            {
                PageContentTemplate updatedPageContent = JsonHelper.Deserialize<PageContentTemplate>(request.JsonValue);
                updatedPageContent.contentId = request.PageContentId;

                ErrorOr<Updated> updateResponse = await _pageContentRepository.UpdatePageContent(updatedPageContent);
                if (updateResponse.IsError)
                {
                    throw new NotImplementedException(updateResponse.FirstError.Description);
                }

                // Return something meaningful later
                return new UpdateContentPageResponse();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
