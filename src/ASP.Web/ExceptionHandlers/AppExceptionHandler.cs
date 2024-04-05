using ASP.Infrastructure.TableStorage;
using Microsoft.AspNetCore.Diagnostics;
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
                var tableClient = await tableStorageProvider.GetTable("ASPProdErrors");

                var appException = CreateTableEntry(httpContext, exception);

                await tableClient.AddEntityAsync(appException);
            }

            return false;
        }

        private AppException CreateTableEntry(HttpContext httpContext, Exception exception)
        {
            var appException = new AppException()
            {
                RowKey = httpContext.TraceIdentifier,
                PartitionKey = "123",
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Type = exception.GetType().Name,
                Title = "",
                Detail = exception.StackTrace,
            };

            return appException;
        }
    }
}
