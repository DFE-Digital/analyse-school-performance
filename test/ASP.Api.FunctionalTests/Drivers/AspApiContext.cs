using ASP.Core;
using ASP.Test.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Newtonsoft.Json;
using System.Reflection;
using TechTalk.SpecFlow.Infrastructure;
using Xunit.Sdk;

namespace ASP.Api.AcceptanceTests.Drivers
{
    public class AspApiContext
    {
        private static readonly MemoryStore _store = new MemoryStore();
        private static readonly IHost _host;

        private readonly ISpecFlowOutputHelper _outputHelper;

        private IDocumentDatabase? _documentDatabase = null;

        private HttpRequest? _lastRequest = null;
        private ApiResult? _lastResponse = null;
        private static readonly Dictionary<string, Func<HttpRequest, Task<ApiResult>>> _functions;
        private static readonly List<Type> _functionTypes;

        static AspApiContext()
        {
            var functionType = typeof(ApiFunction);
            _functionTypes = functionType.Assembly.GetTypes()
                .Where(t => t != functionType && t.IsAssignableTo(functionType))
                .ToList();

            var builder = new HostBuilder();
            new Startup().Configure(builder);
            Configure(builder);
            _host = builder.Build();

            _functions = _functionTypes.ToDictionary(
                t =>
                {
                    var runMethod = t.GetMethod("Run")
                        ?? throw new XunitException($@"Function ""{t.Name}"" does not have a Run method");

                    var functionAttribute = runMethod.GetCustomAttribute<FunctionAttribute>()
                        ?? throw new XunitException($@"Function ""{t.Name}"" does not have a FunctionAttribute on its Run method");

                    return functionAttribute.Name;
                },
                t => (Func<HttpRequest, Task<ApiResult>>)(async (HttpRequest req) =>
                {
                    var function = (ApiFunction)_host.Services.GetService(t)!;
                    return await function.Run(req);
                })
            );
        }

        public HttpRequest LastRequest
        {
            get
            {
                if (_lastRequest == null)
                {
                    AssertWithMessage.NotNull(_lastResponse, "No HTTP request initiated yet. Is the test missing an action?");
                }

                return _lastRequest!;
            }
        }

        public ApiResult LastResponse
        {
            get
            {
                if (_lastResponse == null)
                {
                    AssertWithMessage.NotNull(_lastResponse, "No API response received. Is the test missing an action?");
                }

                _outputHelper.WriteLine("Response content:");
                _outputHelper.WriteLine(JsonConvert.SerializeObject(_lastResponse, Formatting.Indented));

                return _lastResponse!;
            }
        }

        public IDocumentDatabase DocumentDatabase
        {
            get
            {
                if (_documentDatabase != null)
                {
                    return _documentDatabase;
                }

                using (var scope = _host.Services.CreateScope())
                {
                    _documentDatabase = scope.ServiceProvider.GetService<IDocumentDatabase>()!;
                    return _documentDatabase;
                }
            }
        }

        public AspApiContext(ISpecFlowOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public async Task Run(string function, HttpRequest request)
        {
            Func<HttpRequest, Task<ApiResult>> func =
                _functions.TryGetValue(function, out var runMethod)
                    ? runMethod
                    : _ => Task.FromResult(new ApiResult(404, $"Function {function} not found."));

            _lastRequest = request;
            _lastResponse = await func(request);
            request.HttpContext.RequestServices = new ApiServiceProvider();
            await _lastResponse.ExecuteResultAsync(new ActionContext { HttpContext = request.HttpContext });
        }

        private static void Configure(IHostBuilder builder)
        {
            var testMode = Environment.GetEnvironmentVariable("ASP_Test_Mode") ?? "Development";
            var path = Path.GetDirectoryName(typeof(AspApiContext).Assembly.GetAssemblyLocation());

            builder.ConfigureServices(services =>
            {
                foreach (var functionType in _functionTypes)
                {
                    services.AddSingleton(functionType);
                }

                services.Add(new ServiceDescriptor(typeof(MemoryStore), _store));

                if (testMode == "Development")
                {
                    services.RemoveAll<IDocumentDatabase>();
                    services.AddSingleton<IDocumentDatabase, InMemoryDocumentDatabase>();
                }
            });

            builder.ConfigureAppConfiguration(configure =>
            {
                var config = configure
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.Test.json", false);

                if (testMode == "Integration")
                {
                    config.AddJsonFile("appsettings.Test.local.json", true);
                }
            });
        }

        private class ApiServiceProvider : IServiceProvider
        {
            public object? GetService(Type serviceType)
            {
                if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IActionResultExecutor<>))
                {
                    return Activator.CreateInstance(typeof(ActionResultExecutor<>).MakeGenericType(serviceType.GenericTypeArguments[0]));
                }

                throw new NotImplementedException();
            }
        }

        private class ActionResultExecutor<T> : IActionResultExecutor<T> where T : IActionResult
        {
            public Task ExecuteAsync(ActionContext context, T result)
            {
                return Task.CompletedTask;
            }
        }
    }
}
