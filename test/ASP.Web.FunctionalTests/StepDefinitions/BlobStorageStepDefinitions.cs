using ASP.Core;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Web.FunctionalTests.StepDefinitions
{
    [Binding]
    public partial class BlobStorageStepDefinitions : Test.SpecFlow.BlobStorageStepDefinitions
    {
        public BlobStorageStepDefinitions(IBlobStorage blobStorage, ISpecFlowOutputHelper outputHelper, ScenarioContext scenarioContext)
            : base(blobStorage, outputHelper, scenarioContext)
        {
        }
    }
}
