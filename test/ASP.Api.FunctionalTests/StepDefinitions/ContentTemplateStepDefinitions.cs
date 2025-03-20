using ASP.Infrastructure.DocumentDatabase;

namespace ASP.Api.FunctionalTests.StepDefinitions
{
    [Binding]
    public partial class ContentTemplateStepDefinitions : Test.Reqnroll.ContentTemplateStepDefinitions
    {
        public ContentTemplateStepDefinitions(IDocumentDatabase documentDatabase, IReqnrollOutputHelper outputHelper, ScenarioContext scenarioContext)
            : base(documentDatabase, outputHelper, scenarioContext)
        {
        }
    }
}
