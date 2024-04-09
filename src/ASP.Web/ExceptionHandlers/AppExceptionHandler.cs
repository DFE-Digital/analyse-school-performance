using ASP.Infrastructure.TableStorage;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ASP.Web.ExceptionHandlers
{
    public class AppExceptionHandler(ITableStorageProvider tableStorageProvider) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                var tableClient = await tableStorageProvider.GetTable("ASPExceptions");

                var appException = CreateTableEntry(httpContext, exception);

                await tableClient.AddEntityAsync(appException, cancellationToken);
            }

            return false;
        }

        private static AppException CreateTableEntry(HttpContext httpContext, Exception exception)
        {
            var appException = new AppException()
            {
                RowKey = httpContext.TraceIdentifier,
                PartitionKey = "500",
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Type = exception.GetType().Name,
                Title = exception.Message,
                Detail = exception.StackTrace,
            };

            return appException;
        }
    }
}
