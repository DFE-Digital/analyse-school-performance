namespace ASP.Web.FunctionalTests.Drivers
{
    public interface IHtmlPage : IDisposable
    {
        string Path { get; }
        public Task<string> TitleAsync();
        public Task<string> PageContentAsync();

        Task<IElementDriver> ElementAsync(string selector);
        Task<IElementDriver> ElementByLabelAsync(string labelText);
        Task<IElementsDriver> ElementsAsync(string selector);
        Task WaitForSelectorAsync(string selector, string errorIfNotExists);
    }
}
