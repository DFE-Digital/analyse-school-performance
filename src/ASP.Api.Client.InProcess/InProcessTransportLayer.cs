using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Azure.Functions.Worker;
using System.Net;
using System.Reflection;

namespace ASP.Api.Client.InProcess
{
    /// <summary>
    /// Implementation of the <see cref="ITransportLayer "/> abstraction for the <see cref="AspApiClient"/> that
    /// uses the application's <see cref="IServiceProvider"/> to instantiate <see cref="ApiFunction"/> objects
    /// and connect to them directly. This provides all the functionality of the API, including serialization of
    /// request/repsonse body objects, and handling connection errors, without the latency of a real HTTP connection. 
    /// This can be used in automated tests or contexts where performance is paramount, e.g. in production.
    /// </summary>
    public class InProcessTransportLayer : ITransportLayer
    {
        public static List<Type> FunctionTypes { get; }

        private readonly List<FunctionRoute> _functions;

        static InProcessTransportLayer()
        {
            var functionType = typeof(ApiFunction);

            FunctionTypes = functionType.Assembly.GetTypes()
                .Where(t => t != functionType && t.IsAssignableTo(functionType))
                .ToList();
        }

        private readonly IServiceProvider _serviceProvider;

        public InProcessTransportLayer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _functions = FunctionTypes.Select(t => {
                var runMethod = t.GetMethod("Run")
                    ?? throw new InvalidOperationException($@"Function ""{t.Name}"" does not have a Run method");

                var functionAttribute = runMethod.GetCustomAttribute<FunctionAttribute>()
                    ?? throw new InvalidOperationException($@"Function ""{t.Name}"" does not have a FunctionAttribute on its Run method");

                var parameter = runMethod.GetParameters().FirstOrDefault(p => p.ParameterType == typeof(HttpRequest))
                    ?? throw new InvalidOperationException($@"Run method for function ""{t.Name}"" does not have a HttpRequest parameter");

                var httpTriggerAttribute = parameter.GetCustomAttribute<HttpTriggerAttribute>()
                    ?? throw new InvalidOperationException($@"Run method for function ""{t.Name}"" does not have a HttpTrigger attribute on its HttpRequest parameter");

                return new FunctionRoute(functionAttribute.Name, httpTriggerAttribute.Route, httpTriggerAttribute.Methods,
                    async (req) => await Run(t, req));

            }).ToList();
        }

        private async Task<ActionResult> Run(Type type, HttpRequest req)
        {
            if (_serviceProvider.GetService(type) is ApiFunction function)
            {
                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    return await function.Run(req, cancellationTokenSource.Token);
                }
            }

            return new ApiResult(500, $"Could not instantiate type \"{type.Name}\"");
        }

        public async Task<HttpResponseMessage> ExecuteRequest(HttpRequestMessage request)
        {
            var path = request.RequestUri?.AbsolutePath;

            var httpRequest = new DefaultHttpContext().Request;
            httpRequest.Method = request.Method.ToString();
            httpRequest.Path = path;
            httpRequest.QueryString = QueryString.FromUriComponent(request.RequestUri?.Query ?? "");
            if (request.Content != null)
            {
                httpRequest.Body = await request.Content.ReadAsStreamAsync();
            }

            Func<HttpRequest, Task<ActionResult>> func =
                req => _functions.FirstOrDefault(f => f.IsMatch(httpRequest))?.Run(req) ?? 
                    Task.FromResult((ActionResult)new ApiResult(404, $"Not found: Function not found for path: {path}"));

            var result = await func(httpRequest);
            httpRequest.HttpContext.RequestServices = new RequestServiceProvider();

            await result.ExecuteResultAsync(new ActionContext { HttpContext = httpRequest.HttpContext });

            var httpResponse = httpRequest.HttpContext.Response;

            var response = new HttpResponseMessage((HttpStatusCode)httpResponse.StatusCode) {
                Content = new StreamContent(httpResponse.Body)
            };

            foreach (var header in httpResponse.Headers)
            {
                try
                {
                    response.Headers.Add(header.Key, header.Value.AsEnumerable());
                }
                catch (InvalidOperationException)
                {
                    response.Content.Headers.Add(header.Key, header.Value.AsEnumerable());
                }
            }

            return response;
        }

        private class RequestServiceProvider : IServiceProvider
        {
            public object? GetService(Type serviceType)
            {
                if (serviceType == typeof(IActionResultExecutor<ContentResult>))
                {
                    return Activator.CreateInstance(typeof(ContentResultExecutor));
                }

                if (serviceType == typeof(IActionResultExecutor<FileStreamResult>))
                {
                    return Activator.CreateInstance(typeof(FileStreamResultExecutor));
                }

                throw new NotImplementedException();
            }
        }

        private class ContentResultExecutor : IActionResultExecutor<ContentResult>
        {
            public async Task ExecuteAsync(ActionContext context, ContentResult result)
            {
                var response = context.HttpContext.Response;
                response.StatusCode = result.StatusCode ?? 200;
                response.ContentType = result.ContentType;
                response.ContentLength = result.Content?.Length ?? 0;
                response.Body = new MemoryStream();
                await response.WriteAsync(result.Content ?? "");
                response.Body.Position = 0;
            }
        }

        private class FileStreamResultExecutor : IActionResultExecutor<FileStreamResult>
        {
            public Task ExecuteAsync(ActionContext context, FileStreamResult result)
            {
                var response = context.HttpContext.Response;
                response.StatusCode = 200;
                response.ContentType = result.ContentType;
                response.Headers.Append("Content-Disposition", $"attachment; filename={result.FileDownloadName}; filename*=UTF-8''{result.FileDownloadName}");
                response.Body = result.FileStream;
                return Task.CompletedTask;
            }
        }
    }
}
