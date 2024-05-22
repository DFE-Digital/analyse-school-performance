using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace ASP.Web.Features.UrlRewriting
{
    public static class UrlRewriter
    {
        /// <summary>
        /// As per GOV.UK guidelines we would like to be able to use URLs without trailing slashes 
        /// - see https://www.gov.uk/guidance/content-design/url-standards-for-gov-uk#default-id-0f36ff47-heading-1
        /// Unfortunately this does not work well with links to relative paths. For example, if the current
        /// path is "/school/123456/" and there is a link with a relative path to a child page e.g. "phonics"
        /// this will correctly resolve to "/school/123456/phonics". However, if the current
        /// path is "/school/123456" (i.e. without the trailing slash), the same link will resolve to
        /// "/school/phonics" which is incorrect. To fix this and have subdirectories work nicely without having to 
        /// resort to custom logic to render link paths or to have to make all links relative to the root, we just
        /// need to ensure all paths to subdirectories get rewritten to have a trailing slash.
        /// </summary>
        /// <param name="ctx">The <c>RewriteContext</c> passed from RewriteOptions.Add()</param>
        public static void EnsureTrailingSlashOnSubdirectory(RewriteContext ctx)
        {
            var request = ctx.HttpContext.Request;
            var response = ctx.HttpContext.Response;

            string path;
            string[] parts;

            if (!request.Path.HasValue
                // Ignore subdirectories ending in a trailing slash
                || (path = request.Path.Value).EndsWith('/')
                || (parts = path.Split('/')).Length == 0
                // or paths to files (e.g. /path/to/file.js)
                || parts.Last().Contains('.'))
            {
                return;
            }

            // We want to use a permanent redirect (301) as this will be cached within the browser eliminating the reload time
            // for the same page in the future. However, POST requests are not allowed to be rewritten via a permanent redirect so a new
            // POST request is created by the browser, losing the POST data. In this case we need to perform a temporary redirect (307)
            // instead
            response.StatusCode = request.Method == HttpMethods.Post ? (int)HttpStatusCode.TemporaryRedirect : (int)HttpStatusCode.MovedPermanently;

            // Set the redirect path
            response.Headers[HeaderNames.Location] = $"{path}/{request.QueryString}";

            // End the response
            ctx.Result = RuleResult.EndResponse;
        }
    }
}