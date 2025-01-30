namespace ASP.Web.Features.Search;

using ASP.Core.Configuration;

public static class ConfigureSearchExtensions
{
    public static IServiceCollection ConfigureSearch(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<SearchOptions>(configuration, out var config);

        return services;
    }
}