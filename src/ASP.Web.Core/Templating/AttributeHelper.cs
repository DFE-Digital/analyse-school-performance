namespace ASP.Web.Core.Templating
{
    public class AttributeHelper
    {
        private const string Self = "_self";
        private const string Blank = "_blank";
        private readonly IRequestHostProvider _requestHostProvider;

        public AttributeHelper(IRequestHostProvider requestHostProvider)
        {
            _requestHostProvider = requestHostProvider ??
                throw new ArgumentNullException(nameof(requestHostProvider));
        }

        public string SetTargetAttribute(string url)
        {
            if (string.IsNullOrEmpty(url))
                return Self;


            string serverHost = _requestHostProvider.RequestHost;

            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                if (uri.Host != serverHost)
                    return Blank;
            }

            return Self;
        }
    }
}
