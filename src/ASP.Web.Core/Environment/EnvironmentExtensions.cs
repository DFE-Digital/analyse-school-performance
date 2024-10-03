using ASP.Web.Core.ErrorHandling;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ASP.Web.Core.Environment
{
    public static class EnvironmentExtensions
    {
        /// <summary>
        /// Determines if the production error page should be used based on the configuration settings.
        /// </summary>
        /// <param name="environment">The host environment.</param>
        /// <param name="options">The error handling options.</param>
        /// <returns>
        /// A boolean value indicating whether the production error page should be used. Returns <c>true</c> if the 
        /// "ForceProductionErrorPage" setting is enabled; otherwise, <c>false</c>.
        /// </returns>
        public static bool ShouldUseProductionErrorPage(this IHostEnvironment environment, ErrorHandlingOptions options)
        {
            return environment.IsProduction() || options.ForceProductionErrorPage;
        }

        public static bool ShouldShowErrorMessage(this IHostEnvironment environment)
        {
            return !environment.IsProduction();
        }

        /// <summary>
        /// This method checks if the current environment name matches the "Local" environment.
        /// </summary>
        /// <param name="environment">The <see cref="IHostEnvironment"/> representing the current hosting environment.</param>
        /// <returns>
        /// <c>true</c> if the current environment is set to "Local"; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLocalDevelopment(this IHostEnvironment environment)
        {
            return environment.IsEnvironment("Local");
        }

        /// <summary>
        /// This method checks if the current environment name matches the "Test" environment.
        /// </summary>
        /// <param name="environment">The <see cref="IHostEnvironment"/> representing the current hosting environment.</param>
        /// <returns>
        /// <c>true</c> if the current environment is set to "Test"; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTest(this IHostEnvironment environment)
        {
            return environment.IsEnvironment("Test");
        }
    }
}
