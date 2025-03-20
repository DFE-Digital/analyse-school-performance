using ASP.Web.FunctionalTests.Drivers;
using Reqnroll.BoDi;

namespace ASP.Web.FunctionalTests.Support
{
    [Binding]
    public class CurrentTimeSupport
    {
        private readonly IObjectContainer _objectContainer;
        private readonly AspWebContext _web;

        public CurrentTimeSupport(IObjectContainer objectContainer, AspWebContext web)
        {
            _objectContainer = objectContainer;
            _web = web;
        }

        [BeforeScenario]
        public void InitializeRepository()
        {
            _objectContainer.RegisterInstanceAs(_web.CurrentTimeProvider);
        }

        [BeforeScenario]
        public void ResetCurrenTime()
        {
            _web.CurrentTimeProvider.Override = null;
        }
    }
}
