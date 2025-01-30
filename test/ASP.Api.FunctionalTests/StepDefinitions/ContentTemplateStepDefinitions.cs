using ASP.Infrastructure.DocumentDatabase;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.FunctionalTests.StepDefinitions
{
    [Binding]
    public partial class ContentTemplateStepDefinitions : Test.SpecFlow.ContentTemplateStepDefinitions
    {
        public ContentTemplateStepDefinitions(IDocumentDatabase documentDatabase, ISpecFlowOutputHelper outputHelper, ScenarioContext scenarioContext)
            : base(documentDatabase, outputHelper, scenarioContext)
        {
        }
    }
}
