using ASP.Core.Results;
using ASP.Infrastructure.Blob;
using Reqnroll;
using Xunit;

namespace ASP.Test.Reqnroll
{
    [Binding]
    public class BlobStorageStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IBlobStorage _blobStorage;
        private readonly IReqnrollOutputHelper _outputHelper;

        public BlobStorageStepDefinitions(IBlobStorage blobStorage, IReqnrollOutputHelper outputHelper, ScenarioContext scenarioContext)
        {
            _blobStorage = blobStorage;
            _outputHelper = outputHelper;
            _scenarioContext = scenarioContext;
        }

        [Given(@"no files exist in blob storage")]
        public void GivenNoFilesExistInBlobStorage()
        {
            _blobStorage.ClearAsync();
        }

        [Given(@"no blob storage files exist in (.+) container")]
        public void GivenNoFilesExistInBlobStorage(string container)
        {
            _blobStorage.ClearContainer(container);
        }

        [Given(@"blob storage file (.+) exists in (.+) container:")]
        public async Task GivenBlobStorageFileExistsInContainer(string filePath, string container, string fileContents)
        {
            await _blobStorage.UploadAsync(container, filePath, BinaryData.FromString(fileContents));
        }

        [Then(@"blob storage file (.+) should exist in (.+) container:")]
        public async Task ThenBlobStorageFileShouldExistInContainer(string filePath, string container, string expectedFileContents)
        {
            var actualFileContents = await _blobStorage.DownloadAsync(container, filePath)
                .Map(contents => contents.ToString());

            Assert.Equal(expectedFileContents, actualFileContents);
        }
    }
}
