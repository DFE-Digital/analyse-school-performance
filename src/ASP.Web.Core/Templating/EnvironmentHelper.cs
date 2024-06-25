using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ASP.Web.Core.Templating
{
    public static class EnvironmentHelper
    {
        /// <summary>
        /// Determines if the production error page should be used based on the configuration settings.
        /// </summary>
        /// <param name="configuration">The configuration object to retrieve settings from.</param>
        /// <returns>
        /// A boolean value indicating whether the production error page should be used. Returns <c>true</c> if the 
        /// "ForceProductionErrorPage" setting is enabled; otherwise, <c>false</c>.
        /// </returns>
        public static bool ShouldUseProductionErrorPage(IConfiguration configuration, IHostEnvironment environment)
        {
            return environment.IsProduction() || configuration.GetValue("ForceProductionErrorPage", defaultValue: false);
        }

        public static bool ShouldShowErrorMessage(IHostEnvironment environment)
        {
            return !environment.IsProduction();
        }
    }
}
