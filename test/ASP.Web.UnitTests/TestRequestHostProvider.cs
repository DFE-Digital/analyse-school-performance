using ASP.Web.Services;

namespace ASP.Web.UnitTests
{
    public record TestRequestHostProvider(string RequestHost) : IRequestHostProvider;
}
