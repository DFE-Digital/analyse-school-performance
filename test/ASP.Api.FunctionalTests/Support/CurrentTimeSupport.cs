using ASP.Api.FunctionalTests.Drivers;
using Reqnroll.BoDi;

namespace ASP.Api.FunctionalTests.Support
{
    [Binding]
    public class CurrentTimeSupport
    {
        private readonly IObjectContainer _objectContainer;
        private readonly AspApiContext _api;

        public CurrentTimeSupport(IObjectContainer objectContainer, AspApiContext api)
        {
            _objectContainer = objectContainer;
            _api = api;
        }

        [BeforeScenario]
        public void InitializeProvider()
        {
            _objectContainer.RegisterInstanceAs(_api.CurrentTimeProvider);
        }

        [BeforeScenario]
        public void ResetCurrenTime()
        {
            _api.CurrentTimeProvider.Override = null;
        }
    }
}
