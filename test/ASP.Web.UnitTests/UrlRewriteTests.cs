using ASP.Web.Features.UrlRewriting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace ASP.Web.UnitTests
{
    public class UrlRewriterTests
    {
        [InlineData("/", "")]
        [InlineData("/trailing-slash/", "")]
        [InlineData("/multiple/dirs/with/trailing-slash/", "")]
        [InlineData("/trailing-slash/", "?with=query")]
        [InlineData("/path-to-file.js", "")]
        [InlineData("/path-to/another-file.txt", "")]
        [InlineData("/path-to-file.js", "?with=query")]
        [InlineData("/path-to/another-file.txt", "?with=query")]
        [Theory]
        public void GET_PathToSubdirectoryWithTrailingSlash_OrPathToFile_DoesNotRedirect(string path, string queryString)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Method = HttpMethods.Get;
            httpContext.Request.Path = path;
            httpContext.Request.QueryString = new QueryString(queryString);

            var context = new RewriteContext {
                HttpContext = httpContext
            };

            UrlRewriter.EnsureTrailingSlashOnSubdirectory(context);

            Assert.Equal(RuleResult.ContinueRules, context.Result);
            Assert.Equal(200, context.HttpContext.Response.StatusCode);
            Assert.Empty(context.HttpContext.Response.Headers);
        }

        [InlineData("/no-trailing-slash", "", "/no-trailing-slash/")]
        [InlineData("/multiple/dirs/with/no-trailing-slash", "", "/multiple/dirs/with/no-trailing-slash/")]
        [InlineData("/no-trailing-slash", "?with=query", "/no-trailing-slash/?with=query")]
        [Theory]
        public void GET_PathToSubdirectoryWithNoTrailingSlash_PermanentRedirects(string path, string? queryString, string? expectedPath)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Method = HttpMethods.Get;
            httpContext.Request.Path = path;
            httpContext.Request.QueryString = new QueryString(queryString);

            var context = new RewriteContext {
                HttpContext = httpContext
            };

            UrlRewriter.EnsureTrailingSlashOnSubdirectory(context);

            Assert.Equal(RuleResult.EndResponse, context.Result);
            Assert.Equal(301, context.HttpContext.Response.StatusCode);
            Assert.Equal(expectedPath, (string?)context.HttpContext.Response.Headers[HeaderNames.Location]);
        }

        [InlineData("/", "")]
        [InlineData("/trailing-slash/", "")]
        [InlineData("/multiple/dirs/with/trailing-slash/", "")]
        [InlineData("/trailing-slash/", "?with=query")]
        [InlineData("/path-to-file.js", "")]
        [InlineData("/path-to/another-file.txt", "")]
        [InlineData("/path-to-file.js", "?with=query")]
        [InlineData("/path-to/another-file.txt", "?with=query")]
        [Theory]
        public void POST_PathToSubdirectoryWithTrailingSlash_OrPathToFile_DoesNotRedirect(string path, string queryString)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Method = HttpMethods.Post;
            httpContext.Request.Path = path;
            httpContext.Request.QueryString = new QueryString(queryString);

            var context = new RewriteContext {
                HttpContext = httpContext
            };

            UrlRewriter.EnsureTrailingSlashOnSubdirectory(context);

            Assert.Equal(RuleResult.ContinueRules, context.Result);
            Assert.Equal(200, context.HttpContext.Response.StatusCode);
            Assert.Empty(context.HttpContext.Response.Headers);
        }

        [InlineData("/no-trailing-slash", "", "/no-trailing-slash/")]
        [InlineData("/multiple/dirs/with/no-trailing-slash", "", "/multiple/dirs/with/no-trailing-slash/")]
        [InlineData("/no-trailing-slash", "?with=query", "/no-trailing-slash/?with=query")]
        [Theory]
        public void POST_PathToSubdirectoryWithNoTrailingSlash_TemporaryRedirects(string path, string? queryString, string? expectedPath)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Method = HttpMethods.Post;
            httpContext.Request.Path = path;
            httpContext.Request.QueryString = new QueryString(queryString);

            var context = new RewriteContext {
                HttpContext = httpContext
            };

            UrlRewriter.EnsureTrailingSlashOnSubdirectory(context);

            Assert.Equal(RuleResult.EndResponse, context.Result);
            Assert.Equal(307, context.HttpContext.Response.StatusCode);
            Assert.Equal(expectedPath, (string?)context.HttpContext.Response.Headers[HeaderNames.Location]);
        }
    }
}
