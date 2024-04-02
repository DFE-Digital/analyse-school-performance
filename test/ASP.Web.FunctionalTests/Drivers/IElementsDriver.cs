namespace ASP.AcceptanceTests.Drivers
{
    // Interface for tests to interact with a group of elements on the page
    public interface IElementsDriver
    {
        Task<int> CountAsync();
        Task<IList<string>> TextContentsAsync();
        Task<IList<string>> AttributeValuesAsync(string attributeName);
    }
}
