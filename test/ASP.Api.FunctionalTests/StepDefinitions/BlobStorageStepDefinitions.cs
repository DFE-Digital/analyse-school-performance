using ASP.Infrastructure.Blob;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.FunctionalTests.StepDefinitions
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
