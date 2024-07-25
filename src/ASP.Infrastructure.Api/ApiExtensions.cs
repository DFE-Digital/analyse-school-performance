using ASP.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASP.Infrastructure.Api
{
    public static class ApiExtensions
    {
        public static void ConfigureInProcessApi(this IServiceCollection services)
        {
            services.AddScoped<IAspApiClient, AspApiClient>();
            services.AddScoped<ITransportLayer, InProcessTransportLayer>();
            foreach (var functionType in InProcessTransportLayer.FunctionTypes)
            {
                services.AddScoped(functionType);
            }
            services.AddUseCases();
        }

        public static void ConfigureHttpApi(this IServiceCollection services)
        {
            services.AddScoped<IAspApiClient, AspApiClient>();
            services.AddScoped<ITransportLayer, HttpTransportLayer>();
            services.AddOptions<HttpApiOptions>()
               .Configure<IConfiguration>(
                   (settings, configuration) =>
                       configuration
                           .GetSection(nameof(HttpApiOptions))
                           .Bind(settings));
        }
    }
}
