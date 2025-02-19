using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Domain.DataDownloads
{
    public interface IDataDownloadsScopeValidator
    {
        Result<string> ValidateScopeIdentifier(DataDownloadsScopeType scopeType, string scopeIdentifier);
        Task<Result<DataDownloadsScope>> ValidateScope(DataDownloadsScopeType scopeType, string scopeIdentifier, Optional<int> year);
    }
}
