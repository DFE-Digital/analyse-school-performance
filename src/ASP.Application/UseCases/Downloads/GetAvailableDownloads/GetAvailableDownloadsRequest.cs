using ASP.Core.DataDownloads;
using ASP.Core.Optionality;

namespace ASP.Application.UseCases.Downloads.GetAvailableDownloads
{
    public class GetAvailableDownloadsRequest
    {
        public DataDownloadsScopeType ScopeType { get; set; }
        public string ScopeIdentifier { get; set; }
        public Optional<int> Year { get; set; }

        public GetAvailableDownloadsRequest(DataDownloadsScopeType scopeType, string scopeIdentifier, Optional<int> year)
        {
            ScopeType = scopeType;
            ScopeIdentifier = scopeIdentifier;
            Year = year;
        }
    }
}
