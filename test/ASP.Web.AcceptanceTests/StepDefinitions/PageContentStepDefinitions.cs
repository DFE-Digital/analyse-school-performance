using ASP.Core.PageContent.Repository;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.AcceptanceTests.StepDefinitions
{
    [Binding]
    public partial class PageContentStepDefinitions : Test.Acceptance.Core.PageContentStepDefinitions
    {
        public PageContentStepDefinitions(IPageContentRepository repository, ISpecFlowOutputHelper outputHelper)
            : base(repository, outputHelper)
        {
        }
    }
}
