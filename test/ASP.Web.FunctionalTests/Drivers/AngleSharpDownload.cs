using System.Net;

namespace ASP.Web.FunctionalTests.Drivers
{
    public class AngleSharpDownload : IDownload
    {
        private readonly HttpResponseMessage _response;
        private readonly Stream _stream;
        private IReqnrollOutputHelper _outputHelper;

        public AngleSharpDownload(HttpResponseMessage response, Stream stream, IReqnrollOutputHelper outputHelper)
        {
            _response = response;
            _stream = stream;
            _outputHelper = outputHelper;
        }

        public HttpStatusCode Status => _response.StatusCode;
        public Dictionary<string, string> Headers => _response.Headers.Concat(_response.Content.Headers).ToDictionary(h => h.Key, h => string.Join(",", h.Value));
        public Stream Stream => _stream;

        public void Dispose()
        {
            _stream.Dispose();
            _response.Dispose();
        }
    }
}
