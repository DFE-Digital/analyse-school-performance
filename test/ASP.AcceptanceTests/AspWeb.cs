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

namespace ASP.AcceptanceTests
{
    public class AspWeb
    {
        private readonly HttpClient _client;
        private readonly MemoryStore _store;

        public AspWeb(MemoryStore store)
        {
            _store = store;

            var factory = new CustomWebApplicationFactory<Program>(_store);
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions {
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
            var content = await response.Content.ReadAsStringAsync();
            var document = await BrowsingContext.New()
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

            public CustomWebApplicationFactory(MemoryStore store)
            {
                _store = store;
            }

            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.ConfigureServices(services =>
                {
                    services.Add(new ServiceDescriptor(typeof(MemoryStore), _store));

                    services.RemoveAll<IDocumentDatabase>();
                    services.AddSingleton<IDocumentDatabase, InMemoryDocumentDatabase>();
                });

                builder.UseEnvironment("Development");
                builder.UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(Path.GetDirectoryName(GetType().Assembly.GetAssemblyLocation()))
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
                    .Build()
                );
            }
        }
    }
}