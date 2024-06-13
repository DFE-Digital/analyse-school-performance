using ASP.Core;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.AcceptanceTests.StepDefinitions
{
    [Binding]
    public partial class ContentTemplateStepDefinitions : Test.Acceptance.Core.ContentTemplateStepDefinitions
    {
        public ContentTemplateStepDefinitions(IDocumentDatabase documentDatabase, ISpecFlowOutputHelper outputHelper, ScenarioContext scenarioContext)
            : base(documentDatabase, outputHelper, scenarioContext)
        {
        }
    }
}
