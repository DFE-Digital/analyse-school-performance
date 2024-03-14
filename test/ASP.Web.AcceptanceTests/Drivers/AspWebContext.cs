using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io.Network;
using AngleSharp.Io;
using AngleSharp;
using ASP.Core.Templating.Repository;
using ASP.Test.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using ASP.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ASP.Core;
using Microsoft.Extensions.Configuration;
using ASP.Web.Controllers;
using ASP.Web.AcceptanceTests.Services;

namespace ASP.AcceptanceTests.Drivers
{
    public class AspWebContext
    {
        private static readonly HttpClient _client;
        private static readonly CustomWebApplicationFactory<Program> _factory;

        private IDocument? _lastResponse = null;

        static AspWebContext()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        public IContentTemplateRepository PageContentRepository => _factory.PageContentRepository;
        public TestCookieProvider CookieProvider => _factory.CookieProvider;

        public IDocument LastResponse
        {
            get
            {
                if (_lastResponse == null)
                {
                    AssertWithMessage.NotNull(_lastResponse, "No web response received. Is the test missing an action?");
                }

                return _lastResponse!;
            }
        }

        public async Task GetAsync(string path)
        {
            var response = await _client.GetAsync(path);

            _lastResponse = await GetDocumentAsync(response);
        }

        public async Task SubmitFormAsync(IHtmlFormElement form)
        {
            _lastResponse = await form.SubmitAsync();
        }

        public async Task SubmitFormAsync(IHtmlFormElement form, IHtmlElement element)
        {
            _lastResponse = await form.SubmitAsync(element);
        }

        private async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
        {
            var requester = new HttpClientRequester(_client);
            var config = Configuration.Default.With(requester).WithDefaultLoader();
            var content = await response.Content.ReadAsStringAsync();
            var document = await BrowsingContext.New(config)
                .OpenAsync(ResponseFactory, CancellationToken.None);

            return (IHtmlDocument)document;

            void ResponseFactory(VirtualResponse htmlResponse)
            {
                htmlResponse
                    .Address(response.RequestMessage.RequestUri)
                    .Status(response.StatusCode);

                MapHeaders(response.Headers);
                MapHeaders(response.Content.Headers);

                htmlResponse.Content(content);

                void MapHeaders(HttpHeaders headers)
                {
                    foreach (var header in headers)
                    {
                        foreach (var value in header.Value)
                        {
                            htmlResponse.Header(header.Key, value);
                        }
                    }
                }
            }
        }

        private class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
        {
            private readonly MemoryStore _store;
            private readonly TestCookieProvider _cookieProvider;

            private IContentTemplateRepository? _pageContentRepository = null;
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

            public TestCookieProvider CookieProvider => _cookieProvider;

            public CustomWebApplicationFactory()
            {
                _store = new MemoryStore();
                _cookieProvider = new TestCookieProvider();
            }

            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                var testMode = Environment.GetEnvironmentVariable("ASP_Test_Mode") ?? "Development";
                var path = Path.GetDirectoryName(GetType().Assembly.GetAssemblyLocation());

                builder.ConfigureTestServices(services =>
                {
                    var testAssembly = typeof(ComponentTestController).Assembly;
                    services.AddMvc().AddApplicationPart(testAssembly).AddControllersAsServices();
                    services.Add(new ServiceDescriptor(typeof(MemoryStore), _store));
                    services.RemoveAll<ASP.Web.Services.ICookieProvider>();
                    services.Add(new ServiceDescriptor(typeof(ASP.Web.Services.ICookieProvider), _cookieProvider));


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
        }
    }
}
