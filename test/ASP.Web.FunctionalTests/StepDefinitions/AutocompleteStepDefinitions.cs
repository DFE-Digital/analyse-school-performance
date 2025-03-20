using ASP.Web.FunctionalTests.Drivers;

namespace ASP.Web.FunctionalTests.StepDefinitions;

[Binding]
public class AutocompleteStepDefinitions
{
    private readonly IWebDriver _web;
    private readonly IReqnrollOutputHelper _outputHelper;
    private readonly ScenarioContext _scenarioContext;

    public AutocompleteStepDefinitions(IWebDriver web, IReqnrollOutputHelper outputHelper,
        ScenarioContext scenarioContext)
    {
        _web = web;
        _outputHelper = outputHelper;
        _scenarioContext = scenarioContext;
    }

    [Then(@"the autocomplete results should appear")]
    public async Task WaitForAutocompleteResults()
    {
        await _web.CurrentPage.WaitForSelectorAsync(".autocomplete__menu", "Autocomplete results did not appear");
    }

    [Then(@"there should be (.*) autocomplete items")]
    public async Task VerifyAutocompleteItemCount(int expectedCount)
    {
        var selector = ".autocomplete__option";
        var autocompleteItems = await _web.CurrentPage.ElementAsync(selector);
        await autocompleteItems.ShouldHaveCountAsync(expectedCount,
            (expected, actual) =>
                $@"Found {actual} elements within the component with the selector ""{selector}"", expected {expected}.");
    }
}