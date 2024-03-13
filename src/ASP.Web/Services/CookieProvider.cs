namespace ASP.Web.Services
{
    public class CookieProvider : ICookieProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetCookie(string cookieName)
        {
            var cookie = _httpContextAccessor?.HttpContext?.Request.Cookies[cookieName];

            return cookie;
        }

        public void SetCookie(string cookieKey, string cookieValue)
        {
            CookieOptions options = new()
            {
                Expires = DateTime.Now.AddDays(365),
                Secure = true,
                HttpOnly = true,
            };

            _httpContextAccessor?.HttpContext?.Response.Cookies.Append(cookieKey, cookieValue, options);
        }

        public void ClearCookies() { }
    }
}
