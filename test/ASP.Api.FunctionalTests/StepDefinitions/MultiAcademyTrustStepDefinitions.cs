using ASP.Core;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.FunctionalTests.StepDefinitions;

[Binding]
public partial class MultiAcademyTrustStepDefinitions : Test.SpecFlow.MultiAcademyTrustStepDefinitions
{
    public MultiAcademyTrustStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase documentDatabase,
        ISpecFlowOutputHelper outputHelper
    )
        : base(scenarioContext, documentDatabase, outputHelper)
    {
    }
}