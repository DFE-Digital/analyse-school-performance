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
            var response = await JsonHelper.Deserialize<PageContentTemplate>(request.JsonValue)
                .ThenAsync(async updatedPageContent => { 
                    updatedPageContent.contentId = request.PageContentId;
                    return await _pageContentRepository.Update(updatedPageContent);
                });

            if (response.IsError)
            {
                throw new NotImplementedException(response.FirstError.Description);
            }

            // Return something meaningful later
            return new UpdateContentPageResponse();
        }
    }
}
