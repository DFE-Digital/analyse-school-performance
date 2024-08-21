namespace ASP.Web.Features.Cookies
{
    public static class CookieExtensions
    {
        public static IServiceCollection ConfigureCookies(this IServiceCollection services)
        {
            services.AddScoped<ICookieProvider, CookieProvider>();

            return services;
        }
    }
}
