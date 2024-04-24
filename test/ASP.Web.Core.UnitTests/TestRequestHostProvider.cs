using ASP.Web.Core.Templating;

namespace ASP.Web.Core.UnitTests
{
    public record TestRequestHostProvider(string RequestHost) : IRequestHostProvider;
}
