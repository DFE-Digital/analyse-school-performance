using ASP.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Azure.Functions.Worker;
using System.Reflection;

namespace ASP.Infrastructure.Api
{
    /// <summary>
    /// Implementation of the <see cref="ITransportLayer "/> abstraction for the <see cref="AspApiClient"/> that
    /// uses the application's <see cref="System.IServiceProvider"/> to instantiate <see cref="ApiFunction"/> objects
    /// and connect to them directly. This provides all the functionality of the API, including serialization of
    /// request/repsonse body objects, and handling connection errors, without the latency of a real HTTP connection. 
    /// This can be used in automated tests or contexts where performance is paramount, e.g. in production.
    /// </summary>
    public class InProcessTransportLayer : ITransportLayer
    {
        public static List<Type> FunctionTypes { get; }
        
        private readonly Dictionary<string, Func<HttpRequest, Task<ApiResult>>> _functions;

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
            _functions = FunctionTypes.ToDictionary(
                t =>
                {
                    var runMethod = t.GetMethod("Run")
                        ?? throw new InvalidOperationException($@"Function ""{t.Name}"" does not have a Run method");

                    var functionAttribute = runMethod.GetCustomAttribute<FunctionAttribute>()
                        ?? throw new InvalidOperationException($@"Function ""{t.Name}"" does not have a FunctionAttribute on its Run method");

                    return "/api/" + functionAttribute.Name;
                },
                t => (Func<HttpRequest, Task<ApiResult>>)(async (req) => await Run(t, req))
            );
        }

        private async Task<ApiResult> Run(Type type, HttpRequest req) {
            if(_serviceProvider.GetService(type) is ApiFunction function)
            {
                using (var cancellationTokenSource = new CancellationTokenSource())
                {
                    return await function.Run(req, cancellationTokenSource.Token);
                }
            }

            return new ApiResult(500, $"Could not find function for path: \"{req.Path}\"");
        }

        public async Task<TransportLayerResponse> ExecuteRequest(TransportLayerRequest request)
        {
            var function = request.Path ?? "/";
            var httpRequest = CreateRequest(request.Method ?? "GET", function, request.QueryString ?? "");

            using (new RequestBodyWriter(httpRequest, request.Body ?? ""))
            {
                Func<HttpRequest, Task<ApiResult>> func =
                    _functions.TryGetValue(function, out var runMethod)
                        ? runMethod
                        : _ => Task.FromResult(new ApiResult(404, $"Function {function} not found."));

                var result = await func(httpRequest);
                httpRequest.HttpContext.RequestServices = new RequestServiceProvider();

                await result.ExecuteResultAsync(new ActionContext { HttpContext = httpRequest.HttpContext });

                var httpResponse = httpRequest.HttpContext.Response;
                httpResponse.Body.Position = 0;
                using (var sr = new StreamReader(httpResponse.Body))
                {
                    var response = new TransportLayerResponse {
                        StatusCode = httpResponse.StatusCode,
                        Body = await sr.ReadToEndAsync()
                    };

                    foreach (var header in httpResponse.Headers)
                    {
                        response.Headers[header.Key] = header.Value.ToString() ?? "";
                    }

                    return response;
                }
            }
        }

        private HttpRequest CreateRequest(string method, string path, string queryString)
        {
            var request = new DefaultHttpContext().Request;

            request.Method = method;
            request.Path = path;

            if (queryString != null)
            {
                request.QueryString = new QueryString(queryString);
            }

            return request;
        }

        private class RequestBodyWriter : IDisposable
        {
            private readonly Stream _stream;
            private readonly StreamWriter _writer;

            public RequestBodyWriter(HttpRequest request, string body)
            {
                _stream = new MemoryStream();
                _writer = new StreamWriter(_stream);
                _writer.Write(body);
                _writer.Flush();
                _stream.Position = 0;
                request.Body = _stream;
            }

            public void Dispose()
            {
                _writer.Dispose();
                _stream.Dispose();
            }
        }

        private class RequestServiceProvider : IServiceProvider
        {
            public object? GetService(Type serviceType)
            {
                if (serviceType == typeof(IActionResultExecutor<ContentResult>))
                {
                    return Activator.CreateInstance(typeof(ActionResultExecutor));
                }

                throw new NotImplementedException();
            }
        }

        private class ActionResultExecutor : IActionResultExecutor<ContentResult>
        {
            public async Task ExecuteAsync(ActionContext context, ContentResult result)
            {
                var response = context.HttpContext.Response;
                response.StatusCode = result.StatusCode ?? 200;
                response.ContentType = result.ContentType;
                response.Body = new MemoryStream();
                response.ContentLength = result.Content?.Length ?? 0;
                await response.WriteAsync(result.Content ?? "");
            }
        }
    }
}
