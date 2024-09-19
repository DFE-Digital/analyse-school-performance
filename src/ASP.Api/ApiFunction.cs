using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Api
{
    public abstract class ApiFunction
    {
        public abstract Task<ActionResult> Run(HttpRequest req, CancellationToken cancellationToken);
    }
}