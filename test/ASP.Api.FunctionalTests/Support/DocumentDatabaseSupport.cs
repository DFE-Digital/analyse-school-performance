using ASP.Api.FunctionalTests.Drivers;
using Reqnroll.BoDi;

namespace ASP.Api.FunctionalTests.Support
{
    [Binding]
    public class DocumentDatabaseSupport
    {
        private readonly IObjectContainer _objectContainer;
        private readonly AspApiContext _api;

        public DocumentDatabaseSupport(IObjectContainer objectContainer, AspApiContext api)
        {
            _objectContainer = objectContainer;
            _api = api;
        }

        [BeforeScenario]
        public void InitializeRepository()
        {
            _objectContainer.RegisterInstanceAs(_api.DocumentDatabase);
        }

        [BeforeScenario]
        public async Task ClearDownData()
        {
            await _api.DocumentDatabase.Clear();
        }
    }
}
