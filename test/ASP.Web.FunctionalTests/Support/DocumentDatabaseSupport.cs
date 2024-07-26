using ASP.AcceptanceTests.Drivers;
using BoDi;

namespace ASP.AcceptanceTests.Support
{
    [Binding]
    public class DocumentDatabaseSupport
    {
        private readonly IObjectContainer _objectContainer;
        private readonly AspWebContext _web;

        public DocumentDatabaseSupport(IObjectContainer objectContainer, AspWebContext web)
        {
            _objectContainer = objectContainer;
            _web = web;
        }

        [BeforeScenario]
        public void InitializeRepository()
        {
            _objectContainer.RegisterInstanceAs(_web.DocumentDatabase);
        }

        [BeforeScenario]
        public async Task ClearDownData()
        {
            await _web.DocumentDatabase.DeleteAllAsync("content");
            await _web.DocumentDatabase.DeleteAllAsync("establishments");
            await _web.DocumentDatabase.DeleteAllAsync("local-authorities");
        }
    }
}
