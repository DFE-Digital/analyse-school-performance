using System.IO.Compression;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow;
using Xunit;
using ASP.Core.Extensions;

namespace ASP.Test.SpecFlow
{
    [Binding]
    public abstract class FileDownloadStepDefinitions
    {
        protected readonly ISpecFlowOutputHelper _output;
        private ZipArchive? _zipArchive;
        private string? _fileContents;

        public FileDownloadStepDefinitions(ISpecFlowOutputHelper output)
        {
            _output = output;
        }

        protected ZipArchive ZipArchive
        {
            get
            {
                Assert.NotNull(_zipArchive, "No ZIP file download found - are you missing the step \"Then the response should be a ZIP file download with filename ...\"?");

                return _zipArchive!;
            }
        }

        protected string FileContents
        {
            get
            {
                Assert.NotNull(_fileContents, "No file download found - are you missing the step \"Then the response should be a ... file download with filename ...\"?");

                return _fileContents!;
            }
        }

        protected void DisposeArchive()
        {
            if (_zipArchive != null)
            {
                _zipArchive.Dispose();
                _zipArchive = null;
            }
        }

        protected abstract Dictionary<string, string> Headers { get; }
        protected abstract Task<Stream> StreamAsync();

        [Then(@"the response should be an? (ZIP|CSV|TSV|TXT|XLSX|XML) file download")]
        public async Task ThenTheResponseShouldBeAFileDownload(string fileType)
        {
            await ThenTheResponseShouldBeAFileDownloadWithFilename(fileType, null);
        }

        [Then(@"the response should be an? (ZIP|CSV|TSV|TXT|XLSX|XML) file download with filename (.*)")]
        public async Task ThenTheResponseShouldBeAFileDownloadWithFilename(string fileType, string? filename)
        {
            var headers = Headers;

            if (filename != null)
            {
                Assert.Contains(
                    "Content-Disposition",
                    $"attachment; filename={filename}; filename*=UTF-8''{filename}",
                    headers,
                    StringComparison.InvariantCultureIgnoreCase);
            } else
            {
                Assert.Contains(
                    "Content-Disposition",
                    headers,
                    StringComparison.InvariantCultureIgnoreCase);

                headers.TryGetValue("Content-Disposition", out var value);
                Assert.Contains("attachment;", value, StringComparison.InvariantCultureIgnoreCase);
            }

            var contentType = fileType switch {
                "ZIP" => "application/zip",
                "CSV" => "text/csv",
                "XLSX" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "XML" => "application/xml",
                _ => "text/plain"
            };

            Assert.Contains(
                "Content-Type",
                contentType,
                headers,
                StringComparison.InvariantCultureIgnoreCase);

            var stream = await StreamAsync();

            if (fileType == "ZIP")
            {
                _zipArchive = new ZipArchive(stream);
            } else
            {
                using var reader = new StreamReader(stream);
                _fileContents = await reader.ReadToEndAsync();
            }
        }

        [Then(@"the ZIP file download should contain (\d*) files?")]
        public void ThenTheZipFileDownloadShouldContainFiles(int expectedFileCount)
        {
            Assert.Equal(expectedFileCount, ZipArchive.Entries.Count);
        }

        [Then(@"the ZIP file download should contain the file (.*) with contents:")]
        public async Task ThenTheZipFileDownloadShouldContainTheFileWithContents(string filename, string expectedFileContents)
        {
            var fileEntry = ZipArchive.GetEntry(filename);
            Assert.NotNull(fileEntry, $@"Zip file entry ""{filename}"" was not found");

            using var reader = new StreamReader(fileEntry.Open());
            var actualFileContents = await reader.ReadToEndAsync();

            Assert.Equal(expectedFileContents, actualFileContents);
        }

        [Then(@"the file download should have the contents:")]
        public void ThenTheFileDownloadShouldHaveTheContents(string expectedFileContents)
        {
            Assert.Equal(expectedFileContents, FileContents);
        }
    }
}
