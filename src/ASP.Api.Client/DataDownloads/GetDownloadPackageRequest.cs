namespace ASP.Api.Client.DataDownloads;

public record GetDownloadPackageRequest(FileType FileType, List<string> DownloadIds, DataDownloadsScopeType ScopeType, string ScopeIdentifier);