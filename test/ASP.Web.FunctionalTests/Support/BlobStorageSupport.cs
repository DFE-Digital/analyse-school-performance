using ASP.Web.FunctionalTests.Drivers;
using BoDi;

namespace ASP.Web.FunctionalTests.Support
{
    [Binding]
    public class BlobStorageSupport
    {
        private readonly IObjectContainer _objectContainer;
        private readonly AspWebContext _web;

        public BlobStorageSupport(IObjectContainer objectContainer, AspWebContext web)
        {
            _objectContainer = objectContainer;
            _web = web;
        }

        [BeforeScenario]
        public void InitializeRepository()
        {
            _objectContainer.RegisterInstanceAs(_web.BlobStorage);
        }

        [BeforeScenario]
        public async Task ClearDownData()
        {
            await _web.BlobStorage.ClearAsync();
        }
    }
}
