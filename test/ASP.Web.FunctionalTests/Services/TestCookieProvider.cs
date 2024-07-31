using ASP.Web.Features.Cookies;

namespace ASP.Web.FunctionalTests.Services
{
    public class TestCookieProvider : ICookieProvider
    {
        private readonly Dictionary<string, string> _cookies = [];

        public string? GetCookie(string cookieName)
        {
            _cookies.TryGetValue(cookieName, out string? value);
            return value;
        }

        public void SetCookie(string cookieKey, string cookieValue)
        {
            _cookies[cookieKey] = cookieValue;
        }

        public void ClearCookies()
        {
            _cookies.Clear();
        }
    }
}
