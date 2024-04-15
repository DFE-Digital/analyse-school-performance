using ASP.Core.Results;
using ASP.Infrastructure.TableStorage;
using Azure;

namespace ASP.Web.AcceptanceTests.Services
{
    public class TestTableStorageProvider : ITableStorageProvider
    {
        //  private readonly Dictionary<string, string> _cookies = [];

        public async Task<Result<string>> UpdateTable(TableStorageEntry tableStorageEntry)
        {
            return Result.Success("Message");
            //try
            //{

            //    throw new RequestFailedException("message");
            //    // var table = await CreateTable();

            //    // var response = await _tableClient.AddEntityAsync(tableStorageEntry);

            //    // string location;
            //    // var locationa = response. //.TryGetHeader("location", out location);

            //    //return Result.Success(_tableStorageConfiguration.TableName + " updated with error code " + tableStorageEntry.RowKey);

            //}
            //catch (RequestFailedException exception)
            //{
            //    return Error.Unexpected(exception.Message);
            //}

        }

        //private static IEnumerable<Result<string>> GetResponse()
        //{
        //    yield return Result.Success("True");
        //    yield return Error.Unexpected("Error");
        //}
    }

 
}
