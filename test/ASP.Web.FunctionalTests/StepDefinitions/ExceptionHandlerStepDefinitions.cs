using ASP.AcceptanceTests.Drivers;
using System.Net;

namespace ASP.AcceptanceTests.StepDefinitions
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
        public void ClearTableStorageCookies()
        {
            _web.TableStorageProvider.ClearTableStorage();
        }

        [Then(@"the exception details are added to table storage")]
        public void ExceptionDetailsAreAddedToTableStorage()
        {
            var actual = _web.TableStorageProvider._tableStorageEntry
                           .Any(t => t.PartitionKey == HttpStatusCode.InternalServerError.ToString());

            Assert.True(actual);
        }
    }
}
