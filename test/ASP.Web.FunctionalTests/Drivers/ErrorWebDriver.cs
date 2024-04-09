using System.Net;
using Xunit.Sdk;

namespace ASP.AcceptanceTests.Drivers
{
    // This is the default web driver, which will be used for any test that uses StepDefinitions that interact with the
    // AspWebContext (virtual ASP web application) but does not have either the @Javascript:enabled or @Javascript:disabled
    // attribute. These attributes are required in order to know which web driver to use for the test (see:
    // ASP.AcceptanceTests.Support.WebDriverSupport) so if they are not present on any test we fail the test with an error.
    public class ErrorWebDriver : IWebDriver
    {
        private const string ExceptionMessage = 
            "No web driver configured. Please add either the @Javascript:enabled or the @Javascript:disabled attribute to the test scenario to specify the web driver.";

        public Task NavigateAsync(string path)
        {
            throw new XunitException(ExceptionMessage);
        }

        public HttpStatusCode Status =>
            throw new XunitException(ExceptionMessage);

        public string Path => 
            throw new XunitException(ExceptionMessage);

        public string BaseAddress => 
            throw new XunitException(ExceptionMessage);

        public IElementDriver Element(string selector)
        {
            throw new XunitException(ExceptionMessage);
        }

        public IElementDriver ElementByLabel(string labelText)
        {
            throw new XunitException(ExceptionMessage);
        }

        public Task<string> PageContentAsync()
        {
            throw new XunitException(ExceptionMessage);
        }

        public Task<string> PageTitleAsync()
        {
            throw new XunitException(ExceptionMessage);
        }

        public Task SubmitFormAsync(IElementDriver form)
        {
            throw new XunitException(ExceptionMessage);
        }

        public Task SubmitFormAsync(IElementDriver form, IElementDriver element)
        {
            throw new XunitException(ExceptionMessage);
        }

        public IElementsDriver Elements(string selector)
        {
            throw new XunitException(ExceptionMessage);
        }
    }
}
