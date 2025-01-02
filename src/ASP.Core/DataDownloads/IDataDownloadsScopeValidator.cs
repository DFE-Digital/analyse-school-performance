using ASP.Core.Optionality;
using ASP.Core.Results;

namespace ASP.Core.DataDownloads
{
    public interface IDataDownloadsScopeValidator
    {
        Task<Result<DataDownloadsScope>> ValidateScope(DataDownloadsScopeType scopeType, string scopeIdentifier, Optional<int> year);
    }
}
