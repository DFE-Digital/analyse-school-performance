namespace ASP.Web.Features.ContentSecurityPolicy
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureContentSecurityPolicy(this IServiceCollection services)
        {
            services.AddScoped<INonceService>(serviceProvider => new NonceService(32));

            return services;
        }
    }
}
