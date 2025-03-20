using ASP.Web.FunctionalTests.Drivers;
using Reqnroll.BoDi;

namespace ASP.Web.FunctionalTests.Support
{
    // Any tests that interact with the AspWebContext (virtual ASP web application) must have either the
    // @Javascript:enabled or @Javascript:disabled attribute set in order to know which web driver to use
    // for the test scenario.
    [Binding]
    public class WebDriverSupport
    {
        private readonly IObjectContainer _objectContainer;
        private readonly IReqnrollOutputHelper _outputHelper;

        public WebDriverSupport(IObjectContainer objectContainer, IReqnrollOutputHelper outputHelper)
        {
            _objectContainer = objectContainer;
            _outputHelper = outputHelper;
            ConfigureErrorWebDriver();
        }

        [BeforeScenario]
        public void ConfigureErrorWebDriver()
        {
            // Default web driver that fails the test with an error.
            _objectContainer.RegisterTypeAs<ErrorWebDriver, IWebDriver>();
        }

        [BeforeScenario(tags: "Javascript:disabled")]
        public void ConfigureNoJsWebDriver()
        {
            // Use the AngleSharp web driver for tests that don't need Javascript (most tests)
            _objectContainer.RegisterTypeAs<AngleSharpWebDriver, IWebDriver>();
        }

        [BeforeScenario(tags: "Javascript:enabled")]
        public void ConfigureJsWebDriver()
        {
            // Use the Playwright web driver for tests that exercise Javascript code
            _objectContainer.RegisterTypeAs<PlaywrightWebDriver, IWebDriver>();
        }

        [AfterScenario]
        public async Task DisposeWebDriver()
        {
            // Use the AngleSharp web driver for tests that don't need Javascript (most tests)
            var webDriver = _objectContainer.Resolve<IWebDriver>();
            webDriver.Dispose();
            await webDriver.DisposeAsync();
        }
    }
}
