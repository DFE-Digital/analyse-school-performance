using AngleSharp.Html.Dom;
using System.Net;

namespace ASP.AcceptanceTests
{
    public class AspWebResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public IHtmlDocument HtmlContent { get; set; }
        public string RawContent { get; set; }
    }
}