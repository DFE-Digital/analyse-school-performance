using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                })
                .ConfigureAppConfiguration((context, builder) =>
                 {
                     builder.AddJsonFile(Path.Combine(
                             context.HostingEnvironment.ContentRootPath, "appsettings.json"),
                             optional: false, reloadOnChange: false)
                         .AddJsonFile(Path.Combine(
                             context.HostingEnvironment.ContentRootPath, "appsettings.local.json"),
                             optional: true, reloadOnChange: false)
                         .AddEnvironmentVariables();
                 });

            var host = builder.Build();

            host.Run();
        }
    }
}