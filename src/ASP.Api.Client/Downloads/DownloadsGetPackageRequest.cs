namespace ASP.Api.Client.Downloads;

public record DownloadsGetPackageRequest(FileType FileType, List<string> DownloadIds, DownloadsScopeType ScopeType, string ScopeId);