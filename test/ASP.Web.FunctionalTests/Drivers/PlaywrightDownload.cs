namespace ASP.Web.FunctionalTests.Drivers
{
    public class PlaywrightDownload : IDownload
    {
        private readonly Stream _stream;
        private IReqnrollOutputHelper _outputHelper;

        public PlaywrightDownload(Stream stream, IReqnrollOutputHelper outputHelper)
        {
            _stream = new PlaywrightStreamWrapper(stream);
            _outputHelper = outputHelper;
        }

        public Stream Stream => _stream;

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
