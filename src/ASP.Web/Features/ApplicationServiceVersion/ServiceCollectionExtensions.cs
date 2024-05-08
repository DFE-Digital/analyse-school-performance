namespace ASP.Web.Features.ApplicationServiceVersion
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApplicationServiceVersion(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentVersionProvider, GitCommitHashCurrentVersionProvider>();

            return services;
        }
    }
}
