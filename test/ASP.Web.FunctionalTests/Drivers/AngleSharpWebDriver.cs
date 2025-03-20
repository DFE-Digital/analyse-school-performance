using System.Net;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;
using AngleSharp.Io.Network;

namespace ASP.Web.FunctionalTests.Drivers
{
    // Web driver that uses AngleSharp to interact with the HTML DOM of the web application pages. 
    // This is much faster than using a browser automation tool such as Playwright or Selenium,
    // however it has limited support for some web features such as CSS or Javascript, so the
    // Playwright web driver should be used for those. The vast majority of the tests can use this
    // driver as the application must function correctly with Javascript disabled.
    public class AngleSharpWebDriver: IWebDriver
    {
        private static IBrowsingContext? _browsingContext;
        private readonly AspWebContext _web;
        private readonly IReqnrollOutputHelper _outputHelper;
        private AngleSharpDownload? _lastDownload;
        private AngleSharpPage? _lastPage;
        private AngleSharp.Io.IResponse? _lastResponse;

        public AngleSharpWebDriver(AspWebContext web, IReqnrollOutputHelper outputHelper)
        {
            _web = web;
            _outputHelper = outputHelper;

            // The first instance will take longer to initialize as we spin up the
            // browsing context, but this is static so future instances will have access to it
            // Important: this is not thread-safe, but this does not matter as 
            // parallelization of tests is explicitly disabled, if this changes this
            // code could cause intermittent issues
            if (_browsingContext == null)
            {
                var requester = new HttpClientRequester(web.Client);
                var config = Configuration.Default.With(requester).WithDefaultLoader();
                _browsingContext = BrowsingContext.New(config);
            }
            _browsingContext.AddEventListener(EventNames.Requested, BrowsingContext_Requested);
        }

        private void BrowsingContext_Requested(object sender, Event ev)
        {
            if (ev is RequestEvent r)
            {
                _lastResponse = r.Response;
            }
        }

        public IHtmlPage CurrentPage
        {
            get
            {
                Assert.NotNull(_lastPage, @"No web response received. Is the test missing a ""navigate"" step?");

                return _lastPage!;
            }
        }

        public IDownload CurrentDownload
        {
            get
            {
                Assert.NotNull(_lastDownload, @"No web response received. Is the test missing a ""navigate"" step?");

                return _lastDownload!;
            }
        }

        public async Task NavigateAsync(string path)
        {
            var document = await _browsingContext!.OpenAsync(BaseAddress + path);

            _lastPage = new AngleSharpPage(document, _lastResponse!, this, _outputHelper);
            if (_lastDownload != null)
            {
                _lastDownload.Dispose();
                _lastDownload = null;
            }

            ExpectedStatusCode = 200;
        }

        public async Task CaptureDownloadAsync(string path)
        {
            var response = await _web.Client.GetAsync(path);
            var stream = await response.Content.ReadAsStreamAsync();

            _lastDownload = new AngleSharpDownload(response, stream, _outputHelper);
            if (_lastPage != null)
            {
                _lastPage.Dispose();
                _lastPage = null;
            }

            ExpectedStatusCode = 200;
        }

        public async Task SubmitFormAsync(IHtmlFormElement form, IHtmlElement element)
        {
            var document = await form.SubmitAsync(element);

            _lastPage = new AngleSharpPage(document, _lastResponse!, this, _outputHelper);
            _lastDownload = null;
        }

        public HttpStatusCode StatusCode => 
            _lastPage?.Status ?? _lastDownload?.Status ?? 0;

        public Dictionary<string, string> Headers => 
            (_lastPage?.Headers ?? _lastDownload?.Headers ?? new())
                .ToDictionary(h => h.Key, h => string.Join(",", h.Value));

        public string BaseAddress => 
            _web.Client.BaseAddress?.AbsoluteUri.Trim('/') ?? string.Empty;

        public int ExpectedStatusCode { get; set; }

        public async Task ExpectStatusCode()
        {
            if ((int)StatusCode != 200)
            {
                if (_lastPage != null)
                {
                    var pageContent = await _lastPage.PageContentAsync();
                    if (pageContent.StartsWith("<!DOCTYPE html>"))
                    {
                        var errorMessageElement = _lastPage.Document.DocumentElement.QuerySelector(@"[data-testid=""error-display-message""]");
                        var errorMessage = errorMessageElement?.TextContent?
                            .Trim()
                            .Replace("\\u0022", "\"")
                            .Replace("\\r", "\r")
                            .Replace("\\n", "\n")
                            ?? "(none)";

                        _outputHelper.WriteLine($"Error message: {errorMessage}");

                        var stackTraceElement = _lastPage.Document.DocumentElement.QuerySelector(@"[data-testid=""error-display-stack-trace""]");
                        var stackTrace = stackTraceElement?.TextContent?
                            .Trim()
                            .Replace("\\u0022", "\"")
                            .Replace("\\r", "\r")
                            .Replace("\\n", "\n")
                            ?? "(none)";

                        _outputHelper.WriteLine($"Stack trace: {stackTrace}");
                    }
                    else
                    {
                        _outputHelper.WriteLine($"Full page content:{Environment.NewLine}{Environment.NewLine}{pageContent}");
                    }
                }
            }

            Assert.Equal(ExpectedStatusCode, (int)StatusCode, $"Expected response status to be {ExpectedStatusCode} but was {(int)StatusCode}.");
        }

        public void Dispose()
        {
            if (_browsingContext != null)
            {
                _browsingContext.RemoveEventListener(EventNames.Requested, BrowsingContext_Requested);
            }
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
