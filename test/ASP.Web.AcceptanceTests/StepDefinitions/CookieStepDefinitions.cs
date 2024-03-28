using ASP.AcceptanceTests.Drivers;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.AcceptanceTests.StepDefinitions
{
    // Step definitions that deal with cookies using the TestCookieProvider to set/inspect cookie values
    [Binding]
    public class CookieStepDefinitions
    {
        private readonly AspWebContext _web;
        private readonly ISpecFlowOutputHelper _outputHelper;

        public CookieStepDefinitions(AspWebContext web, ISpecFlowOutputHelper outputHelper)
        {
            _web = web;
            _outputHelper = outputHelper;
        }

        [BeforeScenario]
        public void ClearDownCookies()
        {
            _web.CookieProvider.ClearCookies();
        }

        [Then(@"the cookie ""(.*)"" should be set to ""(.*)""")]
        public void ThenTheCookieShouldBeSetTo(string key, string value)
        {
            var cookieValue = _web.CookieProvider.GetCookie(key);

            Assert.Equal(cookieValue, value);
        }

        [When(@"the cookie ""(.*)"" has been set to ""(.*)""")]
        public void WhenTheCookieHasBeenSetTo(string key, string value)
        {
            _web.CookieProvider.SetCookie(key, value);
        }
    }
}
