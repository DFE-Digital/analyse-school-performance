using System.Net;
using Xunit.Sdk;

namespace ASP.Web.FunctionalTests.Drivers
{
    // This is the default web driver, which will be used for any test that uses StepDefinitions that interact with the
    // AspWebContext (virtual ASP web application) but does not have either the @Javascript:enabled or @Javascript:disabled
    // attribute. These attributes are required in order to know which web driver to use for the test (see:
    // ASP.AcceptanceTests.Support.WebDriverSupport) so if they are not present on any test we fail the test with an error.
    public class ErrorWebDriver : IWebDriver
    {
        private const string ExceptionMessage = 
            "No web driver configured. Please add either the @Javascript:enabled or the @Javascript:disabled attribute to the test scenario to specify the web driver.";

        public string BaseAddress =>
            throw new XunitException(ExceptionMessage);

        public Task NavigateAsync(string path) =>
            throw new XunitException(ExceptionMessage);

        public Task CaptureDownloadAsync(Func<Task> action) =>
            throw new XunitException(ExceptionMessage);

        public Task ExpectStatusCode() =>
            throw new XunitException(ExceptionMessage);

        public HttpStatusCode StatusCode =>
            throw new XunitException(ExceptionMessage);

        public IHtmlPage CurrentPage =>
            throw new XunitException(ExceptionMessage);

        public IDownload CurrentDownload =>
            throw new XunitException(ExceptionMessage);

        public int ExpectedStatusCode
        {
            get => throw new XunitException(ExceptionMessage);
            set => throw new XunitException(ExceptionMessage);
        }

        public Dictionary<string, string> Headers =>
            throw new XunitException(ExceptionMessage);

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync() =>
            ValueTask.CompletedTask;
    }
}
