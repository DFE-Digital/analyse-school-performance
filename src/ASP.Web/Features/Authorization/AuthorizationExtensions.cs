using ASP.Infrastructure.Dsi;
using ASP.Infrastructure.Dsi.DsiApiClient;
using ASP.Infrastructure.Dsi.DsiApiClientProvider;

namespace ASP.Web.Features.Authorization
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(Policy.AddPolicies);

            services
                .AddScoped<ISecurityKeyProvider, SymmetricSecurityKeyProvider>()
                .AddScoped<IDsiApiClient, DsiApiClient>();

            services.AddHttpClient<IDsiApiClientProvider, DsiApiClientProvider>();
            services.Configure<DsiPublicApiConfiguration>(configuration.GetSection(DsiConstants.DsiPublicApiSection));

            return services;
        }
    }
}