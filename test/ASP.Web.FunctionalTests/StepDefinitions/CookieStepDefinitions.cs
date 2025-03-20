using ASP.Web.FunctionalTests.Drivers;

namespace ASP.Web.FunctionalTests.StepDefinitions
{
    // Step definitions that deal with cookies using the TestCookieProvider to set/inspect cookie values
    [Binding]
    public class CookieStepDefinitions
    {
        private readonly AspWebContext _web;
        private readonly IReqnrollOutputHelper _outputHelper;

        public CookieStepDefinitions(AspWebContext web, IReqnrollOutputHelper outputHelper)
        {
            _web = web;
            _outputHelper = outputHelper;
        }

        [BeforeScenario(Order = 0)]
        public void ClearDownCookies()
        {
            _web.CookieProvider.ClearCookies();
        }

        [BeforeScenario(Order = 1)]
        public void SetAcceptedTermsOfUseCookie()
        {
            _web.CookieProvider.SetCookie("AcceptedTermsOfUse", "Accepted");
        }

        [Then(@"the cookie ""(.*)"" should be set to ""(.*)""")]
        public void ThenTheCookieShouldBeSetTo(string key, string expectedValue)
        {
            var actualValue = _web.CookieProvider.GetCookie(key);

            Assert.Equal(expectedValue, actualValue);
        }

        [When(@"the cookie ""(.*)"" has been set to ""(.*)""")]
        [Given(@"the cookie ""(.*)"" has been set to ""(.*)""")]
        public void WhenTheCookieHasBeenSetTo(string key, string value)
        {
            _web.CookieProvider.SetCookie(key, value);
        }
    }
}
