using ASP.Core.Optionality;

namespace ASP.Application.UseCases.Downloads.GetAvailableLADownloads
{
    public class GetAvailableLADownloadsRequest
    {
        public string LaCode { get; set; }
        public Optional<int> Year { get; set; }

        public GetAvailableLADownloadsRequest(string LaCodeParam, Optional<int> year)
        {
            LaCode = LaCodeParam;
            Year = year;
        }
    }
}
