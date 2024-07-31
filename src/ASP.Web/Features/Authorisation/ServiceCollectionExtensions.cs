using ASP.Infrastructure.Constants;
using ASP.Infrastructure.Dsi.DsiApiClient;
using ASP.Infrastructure.Dsi.DsiApiClientProvider;
using ASP.Web.Features.Authorisation.LocalAuthority;
using Microsoft.AspNetCore.Authorization;

namespace ASP.Web.Features.Authorisation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAuthorisation(this IServiceCollection services,
                                                             IConfiguration configuration)
        {
            services.AddAuthorization(Policy.AddPolicies);
            
            services.AddScoped<IAuthorizationHandler, LaUserAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, AccessToAllLasAuthorizationHandler>();
            
            services
                .AddScoped<ISecurityKeyProvider, SymmetricSecurityKeyProvider>()
                .AddScoped<IDsiApiClient, DsiApiClient>();

            services.AddHttpClient<IDsiApiClientProvider, DsiApiClientProvider>();

            services.Configure<DsiPublicApiConfiguration>(configuration.GetSection(DsiConstants.DsiPublicApiSection));

            return services;
        }
    }
}
