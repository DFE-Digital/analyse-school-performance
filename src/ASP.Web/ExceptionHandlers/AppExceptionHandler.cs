using Microsoft.AspNetCore.Diagnostics;

//namespace ASP.Web.ExceptionHandlers
//{
//    public class AppExceptionHandler : IExceptionHandler
//    {
//        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
//        {
//            (int statusCode, string errorMessage) = exception switch
//            {
//                Exception => (403, null),
//                _ => (500, null)

//            };

//            return true;
//        }
//    }
//}
