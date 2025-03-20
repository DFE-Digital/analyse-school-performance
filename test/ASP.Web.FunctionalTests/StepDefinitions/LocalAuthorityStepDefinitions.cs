using ASP.Infrastructure.DocumentDatabase;

namespace ASP.Web.FunctionalTests.StepDefinitions;

[Binding]
public partial class LocalAuthorityStepDefinitions : Test.Reqnroll.LocalAuthorityStepDefinitions
{
    public LocalAuthorityStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase database,
        IReqnrollOutputHelper outputHelper)
        : base(scenarioContext, database, outputHelper)
    {
    }
}