using ASP.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASP.Api.Client;

public static class ApiClientExtensions
{
    public static IServiceCollection ConfigureApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureOptions<ApiOptions>(configuration, out var config)
            .AddScoped<IAspApiClient, AspApiClient>();

        services.RemoveAll<ITransportLayer>();
        services.AddScoped<ITransportLayer, HttpTransportLayer>();

        return services;
    }
}
