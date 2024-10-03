using ASP.Core;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using Xunit;

namespace ASP.Test.SpecFlow
{
    [Binding]
    public class BlobStorageStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IBlobStorage _blobStorage;
        private readonly ISpecFlowOutputHelper _outputHelper;

        public BlobStorageStepDefinitions(IBlobStorage blobStorage, ISpecFlowOutputHelper outputHelper, ScenarioContext scenarioContext)
        {
            _blobStorage = blobStorage;
            _outputHelper = outputHelper;
            _scenarioContext = scenarioContext;
        }

        [Given(@"no files exist in blob storage")]
        public void GivenNoFilesExistInBlobStorage()
        {
        }

        [Given(@"blob storage file (.+) exists in (.+) container:")]
        public async Task GivenBlobStorageFileExistsInContainer(string filePath, string container, string fileContents)
        {
            await _blobStorage.UploadAsync(container, filePath, fileContents);
        }

        [Then(@"blob storage file (.+) should exist in (.+) container:")]
        public async Task ThenBlobStorageFileShouldExistInContainer(string filePath, string container, string expectedFileContents)
        {
            var actualFileContents = await _blobStorage.DownloadAsStringAsync(container, filePath);

            Assert.Equal(expectedFileContents, actualFileContents);
        }
    }
}
