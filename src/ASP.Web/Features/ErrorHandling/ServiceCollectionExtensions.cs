using ASP.Infrastructure.TableStorage;

namespace ASP.Web.Features.ErrorHandling
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ConfigureErrorHandling(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                .AddSingleton<ITableStorageProvider, TableStorageProvider>()
                .AddExceptionHandler<ExceptionHandlerServerError>()
                .Configure<TableStorageConfiguration>(configuration.GetSection("TableStorage"));

            return services;
        }
    }
}
