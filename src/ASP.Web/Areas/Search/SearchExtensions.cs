using ASP.Application;
using ASP.Infrastructure;

namespace ASP.Web.Areas.Search
{
    public static class SearchExtensions
    {
        public static IServiceCollection ConfigureSearch(this IServiceCollection services)
        {
            services.ConfigureSearchStrategyFactory();
            services.ConfigureSearchServices();
            return services;
        }
    }
}