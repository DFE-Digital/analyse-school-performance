using ASP.Core;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.FunctionalTests.StepDefinitions;

[Binding]
public partial class LocalAuthorityStepDefinitions : Test.SpecFlow.LocalAuthorityStepDefinitions
{
    public LocalAuthorityStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase documentDatabase,
        ISpecFlowOutputHelper outputHelper
    )
        : base(scenarioContext, documentDatabase, outputHelper)
    {
    }
}