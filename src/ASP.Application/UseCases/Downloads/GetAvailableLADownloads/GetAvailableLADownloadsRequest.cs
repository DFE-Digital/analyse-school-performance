using ASP.Core.Optionality;

namespace ASP.Application.UseCases.Downloads.GetAvailableLADownloads
{
    public class GetAvailableLADownloadsRequest
    {
        public string Code { get; set; }
        public Optional<int> Year { get; set; }

        public GetAvailableLADownloadsRequest(string code, Optional<int> year)
        {
            Code = code;
            Year = year;
        }
    }
}
