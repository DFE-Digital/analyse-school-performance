namespace ASP.Web.FunctionalTests.Drivers
{
    // Interface for tests to interact with an element on the page
    public interface IElementDriver
    {
        IElementDriver Element(string selector);
        IElementDriver ElementByLabel(string labelText);
        IElementsDriver Elements(string selector);

        Task ShouldHaveCountAsync(int count, Func<int, int, string> errorIfIncorrectCount);
        Task ShouldNotExistAsync(string errorIfExists);
        Task<IElementDriver> ShouldExistAsync(string errorIfNotExists);
        Task<string> TextContentAsync();
        Task<string> ImmediateTextContentAsync();
        Task<string> InnerHtmlAsync();
        Task<string> TagNameAsync();
        Task<string> OuterHtmlAsync();
        Task<string> ValueAsync();
        Task<bool> IsCheckedAsync();
        Task<bool> MatchesAsync(string selector);
        Task<string> AttributeAsync(string attributeName);

        Task SetValueAsync(string value);
        Task SetCheckedAsync(bool isChecked);
        Task ClickAsync();
        Task StartDownloadAsync();
    }
}
