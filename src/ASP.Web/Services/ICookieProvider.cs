namespace ASP.Web.Services
{
    public interface ICookieProvider
    {
        public string? GetCookie(string cookieName);

        public void SetCookie(string cookieKey, string cookieValue);

        public void ClearCookies();
    }
}
