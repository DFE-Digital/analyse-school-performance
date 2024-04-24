using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Features.ApplicationServiceVersion
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApplicationServiceVersion(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentVersionProvider, GitCommitHashCurrentVersionProvider>();

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(CurrentVersionActionFilter));
            });

            return services;
        }
    }
}
