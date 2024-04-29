using ASP.AcceptanceTests.Drivers;
using BoDi;

namespace ASP.AcceptanceTests.Support
{
    [Binding]
    public class EstablishmentRepositorySupport
    {
        private readonly IObjectContainer _objectContainer;
        private readonly AspWebContext _web;

        public EstablishmentRepositorySupport(IObjectContainer objectContainer, AspWebContext web)
        {
            _objectContainer = objectContainer;
            _web = web;
        }

        [BeforeScenario]
        public void InitializeRepository()
        {
            _objectContainer.RegisterInstanceAs(_web.EstablishmentRepository);
        }
    }
}
