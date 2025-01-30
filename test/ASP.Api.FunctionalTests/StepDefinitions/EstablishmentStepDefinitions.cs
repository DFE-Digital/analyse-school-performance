using ASP.Infrastructure.DocumentDatabase;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.FunctionalTests.StepDefinitions;

[Binding]
public partial class EstablishmentStepDefinitions : Test.SpecFlow.EstablishmentStepDefinitions
{
    public EstablishmentStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase documentDatabase,
        ISpecFlowOutputHelper outputHelper
    )
        : base(scenarioContext, documentDatabase, outputHelper)
    {
    }
}