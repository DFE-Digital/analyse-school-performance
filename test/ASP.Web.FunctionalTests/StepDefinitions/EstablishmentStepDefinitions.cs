using ASP.Core;
using ASP.Test.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Web.FunctionalTests.StepDefinitions;

[Binding]
public partial class EstablishmentStepDefinitions : Test.SpecFlow.EstablishmentStepDefinitions
{
    public EstablishmentStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase database,
        ISpecFlowOutputHelper outputHelper)
        : base(scenarioContext, database, outputHelper)
    {
    }
}