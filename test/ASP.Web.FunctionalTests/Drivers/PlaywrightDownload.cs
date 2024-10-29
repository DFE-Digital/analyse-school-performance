using TechTalk.SpecFlow.Infrastructure;

namespace ASP.Web.FunctionalTests.Drivers
{
    public class PlaywrightDownload : IDownload
    {
        private readonly Stream _stream;
        private ISpecFlowOutputHelper _outputHelper;

        public PlaywrightDownload(Stream stream, ISpecFlowOutputHelper outputHelper)
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
