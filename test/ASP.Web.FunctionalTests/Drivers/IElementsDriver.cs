namespace ASP.Web.FunctionalTests.Drivers
{
    // Interface for tests to interact with a group of elements on the page
    public interface IElementsDriver
    {
        Task ShouldHaveCountAsync(int count, Func<int, string> errorIfIncorrectCount);
        Task ShouldNotExistAsync(string errorIfExists);
        Task<IElementsDriver> ShouldExistAsync(string errorIfNotExists);

        Task<int> CountAsync();
        Task<IList<string>> TextContentsAsync();
        Task<IList<string>> AttributeValuesAsync(string attributeName);
        Task<IList<string>> TagNamesAsync();
        Task<IList<string>> ValuesAsync();

        IElementDriver ElementAt(int index);
    }
}
