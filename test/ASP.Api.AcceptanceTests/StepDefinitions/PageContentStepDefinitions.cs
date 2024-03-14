using ASP.Core.Templating.Repository;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.AcceptanceTests.StepDefinitions
{
    [Binding]
    public partial class PageContentStepDefinitions : Test.Acceptance.Core.PageContentStepDefinitions
    {
        public PageContentStepDefinitions(IContentTemplateRepository repository, ISpecFlowOutputHelper outputHelper)
            : base(repository, outputHelper)
        {
        }
    }
}
