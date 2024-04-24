using System.Net;

namespace ASP.AcceptanceTests.Drivers
{
    // Interface for tests to interact with a test web browser (either virtual or real)
    public interface IWebDriver
    {
        Task NavigateAsync(string path);
        HttpStatusCode Status { get; }
        string Path { get; }
        string BaseAddress { get; }
        int ExpectedStatusCode { get; set; }

        Task<string> PageContentAsync();
        Task<string> PageTitleAsync();
        Task<IElementDriver> Element(string selector);
        Task<IElementDriver> ElementByLabel(string labelText);
        Task<IElementsDriver> Elements(string selector);
    }
}
