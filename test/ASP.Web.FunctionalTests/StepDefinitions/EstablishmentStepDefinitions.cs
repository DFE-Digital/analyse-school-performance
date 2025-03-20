using ASP.Infrastructure.DocumentDatabase;

namespace ASP.Web.FunctionalTests.StepDefinitions;

[Binding]
public partial class EstablishmentStepDefinitions : Test.Reqnroll.EstablishmentStepDefinitions
{
    public EstablishmentStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase database,
        IReqnrollOutputHelper outputHelper)
        : base(scenarioContext, database, outputHelper)
    {
    }
}