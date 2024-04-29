using ASP.Api.AcceptanceTests.Drivers;
using ASP.Core.Establishments;
using ASP.Core.Templating;
using BoDi;

namespace ASP.AcceptanceTests.Support
{
    [Binding]
    public class EstablishmentRepositorySupport
    {
        private readonly IObjectContainer _objectContainer;
        private readonly AspApiContext _api;

        public EstablishmentRepositorySupport(IObjectContainer objectContainer, AspApiContext api)
        {
            _objectContainer = objectContainer;
            _api = api;
        }

        [BeforeScenario]
        public void InitializeRepository()
        {
            if (!_objectContainer.IsRegistered<IEstablishmentRepository>())
            {
                _objectContainer.RegisterInstanceAs(_api.EstablishmentRepository);
            }
        }
    }
}
