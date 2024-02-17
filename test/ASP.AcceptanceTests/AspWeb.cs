using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using AngleSharp;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using ASP.Web;
using AngleSharp.Dom;
using ASP.Core;
using ASP.Test.Core;
using AngleSharp.Io.Network;
using Microsoft.AspNetCore.TestHost;
using ASP.Core.PageContent.Repository;
using TechTalk.SpecFlow.Assist;

namespace ASP.AcceptanceTests
{
    public class AspWeb
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public IPageContentRepository PageContentRepository => _factory.PageContentRepository;

        public AspWeb()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }

        public async Task<AspWebResponse> GetAsync(string path)
        {
            var response = await _client.GetAsync(path);

            var (rawContent, htmlContent) = await GetDocumentAsync(response);
            return new AspWebResponse {
                StatusCode = response.StatusCode,
                RawContent = rawContent,
                HtmlContent = htmlContent
            };
        }

        private async Task<(string, IHtmlDocument)> GetDocumentAsync(HttpResponseMessage response)
        {
            var requester = new HttpClientRequester(_client);
            var config = Configuration.Default.With(requester).WithDefaultLoader();
            var content = await response.Content.ReadAsStringAsync();
            var document = await BrowsingContext.New(config)
                .OpenAsync(ResponseFactory, CancellationToken.None);

            return (content, (IHtmlDocument)document);

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

            private IPageContentRepository? _pageContentRepository = null;
            public IPageContentRepository PageContentRepository 
            { 
                get
                {
                    if(_pageContentRepository != null)
                    {
                        return _pageContentRepository;
                    }

                    using (var scope = Services.CreateScope())
                    {
                        _pageContentRepository = scope.ServiceProvider.GetService<IPageContentRepository>()!;
                        return _pageContentRepository;
                    }
                }
            }

            public CustomWebApplicationFactory()
            {
                _store = new MemoryStore();
            }

            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                var testMode = Environment.GetEnvironmentVariable("ASP_Test_Mode") ?? "Development";

                builder.ConfigureTestServices(services =>
                {
                    services.Add(new ServiceDescriptor(typeof(MemoryStore), _store));

                    if (testMode == "Development")
                    {
                        services.RemoveAll<IDocumentDatabase>();
                        services.AddSingleton<IDocumentDatabase, InMemoryDocumentDatabase>();
                    }
                });

                builder.ConfigureAppConfiguration(configure => {
                    var config = configure
                        .SetBasePath(Path.GetDirectoryName(GetType().Assembly.GetAssemblyLocation()))
                        .AddJsonFile("appsettings.Test.json", false);

                    if (testMode == "Integration")
                    {
                        config.AddJsonFile("appsettings.Test.local.json", false);
                    }
                });
            }
        }
    }
}