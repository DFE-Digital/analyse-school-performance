using ASP.Infrastructure.DocumentDatabase;

namespace ASP.Api.FunctionalTests.StepDefinitions;

[Binding]
public partial class MultiAcademyTrustStepDefinitions : Test.Reqnroll.MultiAcademyTrustStepDefinitions
{
    public MultiAcademyTrustStepDefinitions(ScenarioContext scenarioContext, IDocumentDatabase documentDatabase,
        IReqnrollOutputHelper outputHelper
    )
        : base(scenarioContext, documentDatabase, outputHelper)
    {
    }
}