using ASP.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Web.AcceptanceTests.Services
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
