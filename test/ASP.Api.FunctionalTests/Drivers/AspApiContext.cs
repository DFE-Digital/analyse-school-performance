using ASP.Core;
using ASP.Infrastructure.Api;
using ASP.Test.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Api.FunctionalTests.Drivers
{
    public class AspApiContext
    {
        private static readonly MemoryStore _store = new MemoryStore();
        private static readonly IHost _host;
        private static readonly ITransportLayer _transport;

        private readonly ISpecFlowOutputHelper _outputHelper;

        private IDocumentDatabase? _documentDatabase = null;
        private TransportLayerRequest? _lastRequest = null;
        private TransportLayerResponse? _lastResponse = null;

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

        public TransportLayerRequest LastRequest
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

        public TransportLayerResponse LastResponse
        {
            get
            {
                if (_lastResponse == null)
                {
                    AssertWithMessage.NotNull(_lastResponse, "No API response received. Is the test missing an action?");
                }

                _outputHelper.WriteLine("Response content:");
                _outputHelper.WriteLine(_lastResponse.BodyString);

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

        public async Task Run(TransportLayerRequest request)
        {
            _lastRequest = request;
            _lastResponse = await _transport.ExecuteRequest(request);
        }

        private static void Configure(IHostBuilder builder)
        {
            var testMode = Environment.GetEnvironmentVariable("ASP_Test_Mode") ?? "Development";
            var path = Path.GetDirectoryName(typeof(AspApiContext).Assembly.GetAssemblyLocation());

            builder.ConfigureServices(services =>
            {
                services.ConfigureInProcessApi();

                if (testMode == "Development")
                {
                    services.Add(new ServiceDescriptor(typeof(MemoryStore), _store));

                    services.RemoveAll<IDocumentDatabase>();
                    services.AddSingleton<IDocumentDatabase, InMemoryDocumentDatabase>();
                }
            });

            builder.ConfigureAppConfiguration(configure =>
            {
                var config = configure
                    .SetBasePath(path)
                    .AddJsonFile("apisettings.Test.json", false);

                if (testMode == "Integration")
                {
                    config.AddJsonFile("apisettings.Test.local.json", true);
                }
            });
        }
    }
}
