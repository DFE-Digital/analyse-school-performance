using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io.Network;
using ASP.Test.Core;
using TechTalk.SpecFlow.Infrastructure;

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
        private readonly ISpecFlowOutputHelper _outputHelper;
        private IDocument? _lastResponse;

        public AngleSharpWebDriver(AspWebContext web, ISpecFlowOutputHelper outputHelper)
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
        }

        // Clear down the static resources after the test run
        // (not strictly necessary but just being tidy)
        [AfterTestRun]
        public static void DisposeResources()
        {
            if (_browsingContext != null)
            {
                _browsingContext.Dispose();
            }
        }

        [NotNull]
        public IDocument LastResponse
        {
            get
            {
                if (_lastResponse == null)
                {
                    AssertWithMessage.NotNull(_lastResponse, @"No web response received. Is the test missing a ""navigate"" step?");
                }

                return _lastResponse!;
            }
        }

        public int ExpectedStatusCode { get; set; }

        public async Task NavigateAsync(string path)
        {
            var response = await _web.Client.GetAsync(path);

            _lastResponse = await GetDocumentAsync(response);
            ExpectedStatusCode = 200;
        }

        public HttpStatusCode Status => LastResponse.StatusCode;
        public string BaseAddress => _web.Client.BaseAddress!.AbsoluteUri.Trim('/') ?? string.Empty;
        public string Path => LastResponse.Url;
        public Task<string> PageContentAsync() => Task.FromResult(LastResponse.ToHtml());
        public Task<string> PageTitleAsync() => Task.FromResult(LastResponse.Title ?? "");

        public async Task<IElementDriver> Element(string selector)
        {
            await ExpectStatusCode();

            return new AngleSharpElementDriver(LastResponse.DocumentElement, selector, this, _outputHelper);
        }

        public async Task<IElementDriver> ElementByLabel(string labelText)
        {
            await ExpectStatusCode();

            var label = LastResponse.QuerySelectorAll(":scope label").FirstOrDefault(l => l.TextContent.Trim() == labelText.Trim());
            AssertWithMessage.NotNull(label, $@"Could not find a label with the text ""{labelText}"".");

            var fieldSelector = $":scope #{label?.Attributes["for"]?.Value}";

            return new AngleSharpElementDriver(LastResponse.DocumentElement, fieldSelector, this, _outputHelper);
        }

        public async Task<IElementsDriver> Elements(string selector)
        {
            await ExpectStatusCode();

            return new AngleSharpElementsDriver(LastResponse.DocumentElement, selector, this);
        }

        public async Task SubmitFormAsync(IHtmlFormElement form, IHtmlElement element)
        {
            _lastResponse = await form.SubmitAsync(element);
            ExpectedStatusCode = 200;
        }

        private async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var document = await _browsingContext!
                .OpenAsync(htmlResponse =>
                {
                    htmlResponse
                        .Address(response.RequestMessage!.RequestUri)
                        .Status(response.StatusCode);

                    MapHeaders(response.Headers);
                    MapHeaders(response.Content.Headers);

                    htmlResponse.Content(content);

                    void MapHeaders(HttpHeaders headers)
                    {
                        foreach (var header in headers)
                        {
                            foreach (var value in header.Value)
                            {
                                htmlResponse.Header(header.Key, value);
                            }
                        }
                    }
                });

            return (IHtmlDocument)document;
        }

        public async Task ExpectStatusCode()
        {
            if ((int)Status != 200)
            {
                var pageContent = await PageContentAsync();
                if (pageContent.StartsWith("<!DOCTYPE html>"))
                {
                    var errorMessageElement = LastResponse.DocumentElement.QuerySelector(@"[data-testid=""error-display-message""]");
                    var errorMessage = errorMessageElement?.TextContent?
                        .Trim()
                        .Replace("\\u0022", "\"")
                        .Replace("\\r", "\r")
                        .Replace("\\n", "\n") 
                        ?? "(none)";

                    _outputHelper.WriteLine($"Error message: {errorMessage}");

                    var stackTraceElement = LastResponse.DocumentElement.QuerySelector(@"[data-testid=""error-display-stack-trace""]");
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

            AssertWithMessage.Equal(ExpectedStatusCode, (int)Status, $"Expected response status to be {ExpectedStatusCode} but was {(int)Status}.");
        }

        public Task WaitForSelectorAsync(string selector, string errorIfNotExists)
        {
            return Task.CompletedTask;
        }
    }
}
