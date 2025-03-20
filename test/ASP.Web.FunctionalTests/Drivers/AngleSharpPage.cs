using System.Net;
using AngleSharp;
using AngleSharp.Dom;

namespace ASP.Web.FunctionalTests.Drivers
{
    public class AngleSharpPage : IHtmlPage
    {
        private readonly IDocument _page;
        private readonly AngleSharp.Io.IResponse _response;
        private readonly AngleSharpWebDriver _web;
        private IReqnrollOutputHelper _outputHelper;

        public AngleSharpPage(IDocument page, AngleSharp.Io.IResponse response, AngleSharpWebDriver web, IReqnrollOutputHelper outputHelper)
        {
            _page = page;
            _response = response;
            _web = web;
            _outputHelper = outputHelper;
        }

        public IDocument Document => _page;
        public string Path => _page.Url;
        public HttpStatusCode Status => _page.StatusCode;
        public Dictionary<string, string> Headers => 
            _response.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value));

        public Task<string> TitleAsync() => Task.FromResult(_page.Title ?? "");
        public Task<string> PageContentAsync() => Task.FromResult(_page.ToHtml());

        public async Task<IElementDriver> ElementAsync(string selector)
        {
            await _web.ExpectStatusCode();

            return new AngleSharpElementDriver(_page.DocumentElement, selector, this, _web, _outputHelper);
        }

        public async Task<IElementDriver> ElementByLabelAsync(string labelText)
        {
            await _web.ExpectStatusCode();

            var label = _page.QuerySelectorAll(":scope label").FirstOrDefault(l => l.TextContent.Trim() == labelText.Trim());
            Assert.NotNull(label, $@"Could not find a label with the text ""{labelText}"".");

            var fieldSelector = $":scope #{label?.Attributes["for"]?.Value}";

            return new AngleSharpElementDriver(_page.DocumentElement, fieldSelector, this, _web, _outputHelper);
        }

        public async Task<IElementsDriver> ElementsAsync(string selector)
        {
            await _web.ExpectStatusCode();

            return new AngleSharpElementsDriver(_page.DocumentElement, selector, this, _web, _outputHelper);
        }

        public Task WaitForSelectorAsync(string selector, string errorIfNotExists)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _page.Dispose();
            _response.Dispose();
        }
    }
}
