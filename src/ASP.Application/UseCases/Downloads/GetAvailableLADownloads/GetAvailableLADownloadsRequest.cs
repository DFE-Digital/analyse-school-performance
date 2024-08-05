namespace ASP.Application.UseCases.Downloads.GetAvailableLADownloads
{
    public class GetAvailableLADownloadsRequest
    {
        public string LaCode { get; set; }

        public GetAvailableLADownloadsRequest(string LaCodeParam)
        {
            LaCode = LaCodeParam;
        }
    }
}
