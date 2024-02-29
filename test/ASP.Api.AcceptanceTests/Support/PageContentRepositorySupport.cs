using ASP.Api.AcceptanceTests.Drivers;
using BoDi;

namespace ASP.AcceptanceTests.Support
{
    [Binding]
    public class PageContentRepositorySupport
    {
        private readonly IObjectContainer _objectContainer;
        private readonly AspApiContext _api;

        public PageContentRepositorySupport(IObjectContainer objectContainer, AspApiContext api)
        {
            _objectContainer = objectContainer;
            _api = api;
        }

        [BeforeScenario]
        public void InitializeRepository()
        {
            _objectContainer.RegisterInstanceAs(_api.PageContentRepository);
        }
    }
}
