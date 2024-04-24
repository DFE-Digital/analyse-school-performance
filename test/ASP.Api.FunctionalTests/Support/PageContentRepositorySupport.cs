using ASP.Api.AcceptanceTests.Drivers;
using ASP.Core.Templating;
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
            if (!_objectContainer.IsRegistered<IContentTemplateRepository>())
            {
                _objectContainer.RegisterInstanceAs(_api.PageContentRepository);
            }
        }
    }
}
