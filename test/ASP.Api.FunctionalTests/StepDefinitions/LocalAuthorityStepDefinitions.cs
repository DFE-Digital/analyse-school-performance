using ASP.Infrastructure.DocumentDatabase;

namespace ASP.Api.FunctionalTests.StepDefinitions;

[Binding]
public partial class LocalAuthorityStepDefinitions : Test.Reqnroll.LocalAuthorityStepDefinitions
{
    public LocalAuthorityStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase documentDatabase,
        IReqnrollOutputHelper outputHelper
    )
        : base(scenarioContext, documentDatabase, outputHelper)
    {
    }
}