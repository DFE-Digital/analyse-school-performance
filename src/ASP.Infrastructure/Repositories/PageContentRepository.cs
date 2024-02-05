using ASP.Core.PageContent.Repository;
using ErrorOr;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using DfE.Data.ComponentLibrary.Infrastructure.Persistence.CosmosDb.Providers;
using ASP.Core.PageContent;

namespace ASP.Infrastructure.Repositories
{
    public class PageContentRepository : IPageContentRepository
    {
        private const string ContainerKey = "content";
        private readonly ICosmosDbContainerProvider _containerProvider;
        private readonly ILogger<PageContentRepository> _logger;

        public PageContentRepository(ICosmosDbContainerProvider containerProvider, ILogger<PageContentRepository> logger)
        {
            _containerProvider = containerProvider ??
                throw new ArgumentNullException(nameof(containerProvider));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }



        public async Task<ErrorOr<Updated>> UpdatePageContent(PageContentTemplate updatedPageContent)
        {
            try
            {
                Container? container = await _containerProvider.GetContainerAsync(ContainerKey);
                ItemResponse<PageContentTemplate> response = await container
                    .UpsertItemAsync(updatedPageContent, new PartitionKey(updatedPageContent.contentId));

                return Result.Updated;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Error.Failure("PageContentRepository.UpdatePageContent", ex.Message);
            }
        }
    }
}
