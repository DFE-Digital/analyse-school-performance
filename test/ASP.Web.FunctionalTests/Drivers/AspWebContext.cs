using ASP.Core;
using ASP.Infrastructure.TableStorage;
using ASP.Test.Core;
using ASP.Web;
using ASP.Web.AcceptanceTests.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using TechTalk.SpecFlow.Infrastructure;
using ASP.Test.Web.Areas.ComponentTest;
using ASP.Web.Features.Cookies;
using ASP.Test.Web.Areas.ErrorTest;
using ASP.Core.Templating;
using ASP.Core.Establishments;

namespace ASP.AcceptanceTests.Drivers
{
    // Context for tests to access the ASP web application running in a test host using ASP.NET Core WebApplicationFactory.
    // Overrides services wired up in Program.cs with test implementations such as in-memory data store and test cookie provider.
    // The in-memory data store is used to run the test suite locally, but can be configured to use an actual database by setting
    // the environment variable ASP_Test_Mode to Integration instead of Development (default). In this case database configuration
    // is set in appsettings.Test.local.json (or application settings on the build server)
    public class AspWebContext
    {
        // Building and launching the WebApplicationFactory and HttpClient are expensive so these are instantiated only once 
        // per test run. Ideally this would be done using an XUnit CollectionFixture which would only create one instance of the
        // CollectionFixture per test run. However the way SpecFlow interacts with XUnit means that CollectionFixtures do not work
        // as expected and create multiple instances per test run, so we have to resort to using static variables which is pretty ugly.
        // This also means we cannot run tests in parallel, however the performance gain from running tests in parallel is vastly
        // outstripped by the loss of launching more than one WebApplicationFactory.
        private static readonly HttpClient _client;
        private static readonly CustomWebApplicationFactory<Program> _factory;
        private readonly ISpecFlowOutputHelper _output;

        static AspWebContext()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        public AspWebContext(ISpecFlowOutputHelper output)
        {
            _output = output;
            _output.WriteLine($"Test server running on: {ServerAddress}");
        }

        public HttpClient Client => _client;
        public IContentTemplateRepository PageContentRepository => _factory.PageContentRepository;
        public IEstablishmentRepository EstablishmentRepository => _factory.EstablishmentRepository;
        public TestCookieProvider CookieProvider => _factory.CookieProvider;
        public TestTableStorageProvider TableStorageProvider => _factory.TableStorageProvider;

        public string ServerAddress => _factory.ServerAddress;

        private class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
        {
            private IHost? _host;
            private readonly MemoryStore _store;
            private readonly TestCookieProvider _cookieProvider;
            private readonly TestTableStorageProvider _tableStorageProvider;

            private IContentTemplateRepository? _pageContentRepository;
            private IEstablishmentRepository? _establishmentRepository;

            public TestCookieProvider CookieProvider => _cookieProvider;
            public TestTableStorageProvider TableStorageProvider => _tableStorageProvider;


            public CustomWebApplicationFactory()
            {
                _store = new MemoryStore();
                _cookieProvider = new TestCookieProvider();
                _tableStorageProvider = new TestTableStorageProvider();
                ClientOptions.AllowAutoRedirect = true;
            }

            public IContentTemplateRepository PageContentRepository
            {
                get
                {
                    if (_pageContentRepository != null)
                    {
                        return _pageContentRepository;
                    }

                    using (var scope = Services.CreateScope())
                    {
                        _pageContentRepository = scope.ServiceProvider.GetService<IContentTemplateRepository>()!;
                        return _pageContentRepository;
                    }
                }
            }

            public IEstablishmentRepository EstablishmentRepository
            {
                get
                {
                    if (_establishmentRepository != null)
                    {
                        return _establishmentRepository;
                    }

                    using (var scope = Services.CreateScope())
                    {
                        _establishmentRepository = scope.ServiceProvider.GetService<IEstablishmentRepository>()!;
                        return _establishmentRepository;
                    }
                }
            }

            public string ServerAddress
            {
                get
                {
                    EnsureServer();
                    return ClientOptions.BaseAddress.ToString();
                }
            }

            private void EnsureServer()
            {
                if (_host is null)
                {
                    // This forces WebApplicationFactory to bootstrap the server  
                    using var _ = CreateDefaultClient();
                }
            }

            // To allow the in memory test application to be accessible to a browser-based test framework (e.g. Playwright)
            // we need to override the CreateHost() method to create an actual virtual server listening on a real web address
            // instead of http://localhost which is used by the test host
            //
            // More information:
            // * https://danieldonbavand.com/2022/06/13/using-playwright-with-the-webapplicationfactory-to-test-a-blazor-application/
            // * https://github.com/martincostello/dotnet-minimal-api-integration-testing
            protected override IHost CreateHost(IHostBuilder builder)
            {
                // Create the host for TestServer now before we  
                // modify the builder to use Kestrel instead.    
                var testHost = builder.Build();

                // Modify the host builder to use Kestrel instead  
                // of TestServer so we can listen on a real address.    

                builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

                // Create and start the Kestrel server before the test server,  
                // otherwise due to the way the deferred host builder works    
                // for minimal hosting, the server will not get "initialized    
                // enough" for the address it is listening on to be available.    
                // See https://github.com/dotnet/aspnetcore/issues/33846.    

                _host = builder.Build();
                _host.Start();

                // Extract the selected dynamic port out of the Kestrel server  
                // and assign it onto the client options for convenience so it    
                // "just works" as otherwise it'll be the default http://localhost    
                // URL, which won't route to the Kestrel-hosted HTTP server.     

                var server = _host.Services.GetRequiredService<IServer>();
                var addresses = server.Features.Get<IServerAddressesFeature>();

                ClientOptions.AllowAutoRedirect = false;
                ClientOptions.BaseAddress = addresses!.Addresses
                    .Select(x => new Uri(x))
                    .Last();

                // Return the host that uses TestServer, rather than the real one.  
                // Otherwise the internals will complain about the host's server    
                // not being an instance of the concrete type TestServer.    
                // See https://github.com/dotnet/aspnetcore/pull/34702.   

                // Don't think we need to actually start the testHost as we're not using it.
                // Seems to work fine without starting but left it commented out just in case
                // we need it after all - SS 27/03/24
                testHost.Start();
                return testHost;
            }

            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                // ASP_Test_Mode environment variable can be set to "Development" or "Integration"
                // Development: uses in-memory database for speed
                // Integration: uses a real database (can cause contention issues if multiple people
                // are running this at once against the same database)
                // TODO: look into containerisation as a possible solution, see Aasim's PR:
                // https://agilefactory.visualstudio.com/SPT/_git/DfE.Data.PerformanceTables.Services/pullrequest/5046
                var testMode = Environment.GetEnvironmentVariable("ASP_Test_Mode") ?? "Development";
                var path = Path.GetDirectoryName(GetType().Assembly.GetAssemblyLocation());

                builder.ConfigureTestServices(services =>
                {
                    // Add component test controller and views from ASP.Test.Web for isolated testing of components
                    // We can use AddMvcCore() here because it uses IServiceCollection.TryAddEnumerable() behind the scenes which
                    // is idempotent
                    services.AddMvcCore()
                        .AddApplicationPart(typeof(ComponentTestController).Assembly)
                        .AddApplicationPart(typeof(ErrorTestController).Assembly)
                        .AddControllersAsServices();

                    // Add in-memory data store
                    services.Add(new ServiceDescriptor(typeof(MemoryStore), _store));

                    // Add test implementation of cookie provider to control/inspect cookie state
                    services.RemoveAll<ICookieProvider>();
                    services.Add(new ServiceDescriptor(typeof(ICookieProvider), _cookieProvider));
                    // Add test implementation of table storage provider
                    services.RemoveAll<ITableStorageProvider>();
                    services.Add(new ServiceDescriptor(typeof(ITableStorageProvider), _tableStorageProvider));

                    if (testMode == "Development")
                    {
                        // Replace Cosmos implementation already wired up in Program.cs with implementation that uses the
                        // in-memory store
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
                        // Add local config for connection strings to the test database
                        config.AddJsonFile("appsettings.Test.local.json", true);
                    }
                });
            }
        }
    }
}
