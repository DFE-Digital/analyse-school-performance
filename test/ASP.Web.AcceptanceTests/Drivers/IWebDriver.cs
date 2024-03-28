using System.Net;

namespace ASP.AcceptanceTests.Drivers
{
    // Interface for tests to interact with a test web browser (either virtual or real)
    public interface IWebDriver
    {
        Task NavigateAsync(string path);
        HttpStatusCode Status { get; }
        Task<string> PageContentAsync();
        Task<string> PageTitleAsync();
        IElementDriver Element(string selector);
        IElementDriver ElementByLabel(string labelText);
        IElementsDriver Elements(string selector);
    }
}
