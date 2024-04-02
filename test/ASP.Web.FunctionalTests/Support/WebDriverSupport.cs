using ASP.AcceptanceTests.Drivers;
using ASP.Web.AcceptanceTests.Drivers;
using BoDi;

namespace ASP.AcceptanceTests.Support
{
    // Any tests that interact with the AspWebContext (virtual ASP web application) must have either the
    // @Javascript:enabled or @Javascript:disabled attribute set in order to know which web driver to use
    // for the test scenario.
    [Binding]
    public class WebDriverSupport
    {
        private readonly IObjectContainer _objectContainer;

        public WebDriverSupport(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;

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
    }
}
