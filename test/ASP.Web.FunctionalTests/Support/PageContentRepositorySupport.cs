using ASP.AcceptanceTests.Drivers;
using BoDi;

namespace ASP.AcceptanceTests.Support
{
    [Binding]
    public class PageContentRepositorySupport
    {
        private readonly IObjectContainer _objectContainer;
        private readonly AspWebContext _web;

        public PageContentRepositorySupport(IObjectContainer objectContainer, AspWebContext web)
        {
            _objectContainer = objectContainer;
            _web = web;
        }

        [BeforeScenario]
        public void InitializeRepository()
        {
            _objectContainer.RegisterInstanceAs(_web.PageContentRepository);
            _objectContainer.RegisterInstanceAs(_web.EstablishmentRepository);
        }
    }
}
