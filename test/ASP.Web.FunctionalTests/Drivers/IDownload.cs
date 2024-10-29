namespace ASP.Web.FunctionalTests.Drivers
{
    public interface IDownload : IDisposable
    {
        public Stream Stream { get; }
    }
}
