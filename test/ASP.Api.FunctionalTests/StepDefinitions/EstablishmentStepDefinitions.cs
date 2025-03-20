using ASP.Infrastructure.DocumentDatabase;

namespace ASP.Api.FunctionalTests.StepDefinitions;

[Binding]
public partial class EstablishmentStepDefinitions : Test.Reqnroll.EstablishmentStepDefinitions
{
    public EstablishmentStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase documentDatabase,
        IReqnrollOutputHelper outputHelper
    )
        : base(scenarioContext, documentDatabase, outputHelper)
    {
    }
}