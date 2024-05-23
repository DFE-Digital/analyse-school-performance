using ASP.Core;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.AcceptanceTests.StepDefinitions
{
    [Binding]
    public partial class PageContentStepDefinitions : Test.Acceptance.Core.PageContentStepDefinitions
    {
        public PageContentStepDefinitions( 
            IDocumentDatabase documentDatabase, 
            ISpecFlowOutputHelper outputHelper)
            : base(documentDatabase, outputHelper)
        {
        }
    }
}
