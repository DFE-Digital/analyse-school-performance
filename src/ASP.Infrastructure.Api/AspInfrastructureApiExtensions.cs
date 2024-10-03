using ASP.Api;
using ASP.Application;
using ASP.Infrastructure.Blob;
using ASP.Infrastructure.DocumentDatabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Infrastructure.Api
{
    public static class AspInfrastructureApiExtensions
    {
        public static IServiceCollection ConfigureApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureOptions<ApiOptions>(configuration, out var config)
                .AddScoped<IAspApiClient, AspApiClient>();

            if(config.InProcess)
            {
                ConfigureInProcessApi(services, configuration);
            } else
            {
                ConfigureHttpApi(services);
            }

            return services;
        }

        public static IServiceCollection ConfigureInProcessTransportLayer(this IServiceCollection services)
        {
            services.RemoveAll<ITransportLayer>();
            services.AddScoped<ITransportLayer, InProcessTransportLayer>();
            foreach (var functionType in InProcessTransportLayer.FunctionTypes)
            {
                services.AddScoped(functionType);
            }

            return services;
        }

        private static void ConfigureInProcessApi(IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureInProcessTransportLayer()
                .ConfigureDocumentDatabase(configuration)
                .ConfigureBlobStorage(configuration)
                .RegisterUseCases()
                .RegisterRepositories()
                .AddScoped<ApiResultConverter>();
        }

        private static void ConfigureHttpApi(IServiceCollection services)
        {
            services.RemoveAll<ITransportLayer>();
            services.AddScoped<ITransportLayer, HttpTransportLayer>();
        }
    }
}
