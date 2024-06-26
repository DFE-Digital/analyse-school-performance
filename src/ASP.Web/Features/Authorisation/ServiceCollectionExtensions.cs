using ASP.Infrastructure.Constants;
using ASP.Infrastructure.Dsi.DsiApiClient;
using ASP.Infrastructure.Dsi.DsiApiClientProvider;
using System.Security.Claims;

namespace ASP.Web.Authorisation
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAuthorisation(this IServiceCollection services,
                                                             IConfiguration configuration)
        {
            services.AddAuthorization(
                options =>
                     options.AddPolicy("ASP DfE Named", policy => policy.RequireAssertion(context =>
                                    context.User.HasClaim(c =>
                                        c.Type == ClaimTypes.Role && c.Value == "ASP DfE Named"

                                    )))
            );

            services
                .AddScoped<ISecurityKeyProvider, SymmetricSecurityKeyProvider>()
                .AddScoped<IDsiApiClient, DsiApiClient>();

            services.AddHttpClient<IDsiApiClientProvider, DsiApiClientProvider>();

            services.Configure<DsiPublicApiConfiguration>(configuration.GetSection(DsiConstants.DsiPublicApiSection));

            return services;
        }
    }
}
