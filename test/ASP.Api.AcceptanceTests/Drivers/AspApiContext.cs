using ASP.Core.PageContent.Repository;
using ASP.Test.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ASP.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using TechTalk.SpecFlow.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ASP.Api.AcceptanceTests.Drivers
{
    public class AspApiContext
    {
        private static readonly MemoryStore _store = new MemoryStore();
        private static readonly IHost _host;

        private readonly ISpecFlowOutputHelper _outputHelper;

        private IPageContentRepository? _pageContentRepository = null;
        private HttpRequest? _lastRequest = null;
        private ApiResult? _lastResponse = null;

        static AspApiContext()
        {
            var builder = new HostBuilder();
            new Startup().Configure(builder);
            Configure(builder);
            _host = builder.Build();
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

        public IPageContentRepository PageContentRepository
        {
            get
            {
                if (_pageContentRepository != null)
                {
                    return _pageContentRepository;
                }

                using (var scope = _host.Services.CreateScope())
                {
                    _pageContentRepository = scope.ServiceProvider.GetService<IPageContentRepository>()!;
                    return _pageContentRepository;
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
                function switch {
                    "ViewContentPage" => _host.Services.GetService<ViewContentPage>()!.Run,
                    "UpdateContentPage" => _host.Services.GetService<UpdateContentPage>()!.Run,
                    _ => _ => Task.FromResult(new ApiResult(404, $"Function {function} not found."))
                };

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
                services.AddSingleton<ViewContentPage, ViewContentPage>();
                services.AddSingleton<UpdateContentPage, UpdateContentPage>();

                services.Add(new ServiceDescriptor(typeof(MemoryStore), _store));

                if (testMode == "Development")
                {
                    services.RemoveAll<IDocumentDatabase>();
                    services.AddSingleton<IDocumentDatabase, InMemoryDocumentDatabase>();
                }
            });

            builder.ConfigureAppConfiguration(configure => {
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
