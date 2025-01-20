using ASP.Application;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Web.Areas.School;

public abstract class SchoolsController : Controller
{
    protected readonly IAspApiClient _api;
    protected readonly IHostEnvironment _hostEnvironment;

    protected SchoolsController(
        IAspApiClient api,
        IHostEnvironment hostEnvironment
    )
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }
}