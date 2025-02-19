using ASP.Api.Client.InProcess;
using ASP.Core.Time;
using ASP.Infrastructure.Blob;
using ASP.Infrastructure.DocumentDatabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.FunctionalTests.Drivers
{
    public class AspApiContext
    {
        private static readonly IHost _host;
        private static readonly ASP.Api.Client.ITransportLayer _transport;

        private readonly ISpecFlowOutputHelper _outputHelper;
        private HttpRequestMessage? _lastRequest = null;
        private HttpResponseMessage? _lastResponse = null;

        static AspApiContext()
        {

            var builder = new HostBuilder();
            new Startup().Configure(builder);
            Configure(builder);
            _host = builder.Build();
            _transport = new InProcessTransportLayer(_host.Services);
        }

        public AspApiContext(ISpecFlowOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        public HttpRequestMessage LastRequest
        {
            get
            {
                if (_lastRequest == null)
                {
                    Assert.NotNull(_lastResponse, "No HTTP request initiated yet. Is the test missing an action?");
                }

                return _lastRequest!;
            }
        }

        public HttpResponseMessage LastResponse
        {
            get
            {
                if (_lastResponse == null)
                {
                    Assert.NotNull(_lastResponse, "No API response received. Is the test missing an action?");
                }

                return _lastResponse!;
            }
        }

        public IDocumentDatabase DocumentDatabase
        {
            get
            {
                using (var scope = _host.Services.CreateScope())
                {
                    return scope.ServiceProvider.GetService<IDocumentDatabase>()!;
                }
            }
        }

        public IBlobStorage BlobStorage
        {
            get
            {
                using (var scope = _host.Services.CreateScope())
                {
                    return scope.ServiceProvider.GetService<IBlobStorage>()!;
                }
            }
        }

        public CurrentTimeProvider CurrentTimeProvider
        {
            get
            {
                return _host.Services.GetRequiredService<CurrentTimeProvider>();
            }
        }

        public async Task Run(HttpRequestMessage request)
        {
            _lastRequest = request;
            _lastResponse = await _transport.ExecuteRequest(request);
        }

        private static void Configure(IHostBuilder builder)
        {
            var testMode = Environment.GetEnvironmentVariable("ASP_Test_Mode") ?? "Development";
            var path = Path.GetDirectoryName(typeof(AspApiContext).Assembly.GetAssemblyLocation());

            builder.ConfigureAppConfiguration(configure =>
            {
                configure
                    .SetBasePath(path)
                    .AddJsonFile("local.settings.template.test.json", false)
                    // Add local config for connection strings to the test database
                    .AddJsonFile("local.settings.test.json", true);
            });

            builder.ConfigureServices((context, services) => services.ConfigureApiClient(context.Configuration));
        }
    }
}
