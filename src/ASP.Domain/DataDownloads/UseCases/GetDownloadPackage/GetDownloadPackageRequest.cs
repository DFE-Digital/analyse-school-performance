namespace ASP.Domain.DataDownloads.UseCases.GetDownloadPackage
{
    public record GetDownloadPackageRequest(FileType FileType, List<string> DownloadIds, DataDownloadsScopeType ScopeType, string ScopeIdentifier);
}
