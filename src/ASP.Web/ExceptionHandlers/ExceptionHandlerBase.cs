using ASP.Infrastructure.TableStorage;

namespace ASP.Web.ExceptionHandlers
{
    public class ExceptionHandlerBase
    {
        public TableStorageEntry CreateTableStorageEntry(HttpContext httpContext, Exception exception, string partitionKey, int statusCode)
        {
            var tableStorageEntry = new TableStorageEntry()
            {
                RowKey = httpContext.TraceIdentifier,
                PartitionKey = partitionKey,
                StatusCode = statusCode,
                Type = exception.GetType().Name,
                Title = exception.Message,
                Detail = exception.StackTrace,
            };

            return tableStorageEntry;
        }
    }
}
