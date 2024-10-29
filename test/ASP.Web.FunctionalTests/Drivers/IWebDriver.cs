using System.Net;

namespace ASP.Web.FunctionalTests.Drivers
{
    // Interface for tests to interact with a test web browser (either virtual or real)
    public interface IWebDriver: IDisposable, IAsyncDisposable
    {
        string BaseAddress { get; }

        Task NavigateAsync(string path);

        IHtmlPage CurrentPage { get; }
        IDownload CurrentDownload { get; }
        public HttpStatusCode StatusCode { get; }
        public Dictionary<string, string> Headers { get; }

        int ExpectedStatusCode { get; set; }
        Task ExpectStatusCode();
    }
}
