namespace ASP.Application.UseCases.Downloads.LaDownloads
{
    public class GetLaDownloadsUseCaseRequest
    {
        public string LaCode { get; set; }

        public GetLaDownloadsUseCaseRequest(string LaCodeParam)
        {
            LaCode = LaCodeParam;
        }
    }
}
