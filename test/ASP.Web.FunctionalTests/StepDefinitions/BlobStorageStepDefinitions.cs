using ASP.Infrastructure.Blob;

namespace ASP.Web.FunctionalTests.StepDefinitions
{
    [Binding]
    public partial class BlobStorageStepDefinitions : Test.Reqnroll.BlobStorageStepDefinitions
    {
        public BlobStorageStepDefinitions(IBlobStorage blobStorage, IReqnrollOutputHelper outputHelper, ScenarioContext scenarioContext)
            : base(blobStorage, outputHelper, scenarioContext)
        {
        }
    }
}
