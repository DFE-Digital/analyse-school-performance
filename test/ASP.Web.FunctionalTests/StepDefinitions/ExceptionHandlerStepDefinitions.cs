using ASP.Core.Results;
using ASP.Web.FunctionalTests.Drivers;

namespace ASP.Web.FunctionalTests.StepDefinitions
{
    [Binding]
    public class ExceptionHandlerStepDefinitions
    {
        private readonly AspWebContext _web;

        public ExceptionHandlerStepDefinitions(AspWebContext web)
        {
            _web = web;
        }

        [BeforeScenario(Order = 0)]
        public Task ClearTableStorageCookies()
        {
            return _web.TableStorageProvider.Clear();
        }

        [Then(@"an? (.+) entry should be added to table storage")]
        public async Task AnErrorWithStatusCodeWasAddedToTableStorage(string errorType)
        {
            var actual = await _web.TableStorageProvider.Exists(errorType);

            Assert.Equal(Result.Success(true), actual);
        }
    }
}
