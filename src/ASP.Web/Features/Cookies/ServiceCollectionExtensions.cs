namespace ASP.Web.Features.Cookies
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureCookies(this IServiceCollection services)
        {
            services.AddScoped<ICookieProvider, CookieProvider>();

            return services;
        }
    }
}
