using ASP.Core;
using ASP.Test.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Web.FunctionalTests.StepDefinitions;

[Binding]
public partial class LocalAuthorityStepDefinitions : Test.SpecFlow.LocalAuthorityStepDefinitions
{
    public LocalAuthorityStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase database,
        ISpecFlowOutputHelper outputHelper)
        : base(scenarioContext, database, outputHelper)
    {
    }
}