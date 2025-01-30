using ASP.Core.Configuration;
using ASP.Infrastructure.Dsi.DsiApiClient;
using ASP.Infrastructure.Dsi.DsiApiClientProvider;

namespace ASP.Web.Features.Authorization
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureOptions<DsiPublicApiOptions>(configuration)
                .AddAuthorization(Policy.AddPolicies)
                .AddScoped<ISecurityKeyProvider, SymmetricSecurityKeyProvider>()
                .AddScoped<IDsiApiClient, DsiApiClient>()
                .AddHttpClient<IDsiApiClientProvider, DsiApiClientProvider>();

            return services;
        }
    }
}