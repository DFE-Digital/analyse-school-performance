using ASP.Infrastructure.TableStorage;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace ASP.Web.ExceptionHandlers
{
    public class ExceptionHandlerServerError(ITableStorageProvider tableStorageProvider) : ExceptionHandlerBase, IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                var tableClient = await tableStorageProvider.GetTable("ASPExceptions");

                var tableStorageEntry = CreateTableStorageEntry(httpContext, exception,
                    HttpStatusCode.InternalServerError.ToString(),
                    (int)HttpStatusCode.InternalServerError);

                await tableStorageProvider.AddTableEntry(tableClient, tableStorageEntry);
            }

            return false;
        }
    }
}
